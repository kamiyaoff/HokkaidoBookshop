using HokkaidoBookshop.Models;
using HokkaidoBookshop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;

namespace HokkaidoBookshop.Controllers {
	public class UserController : Controller 
	{
		private readonly ApplicationContext db;
		private readonly IWebHostEnvironment hostEnvironment;

		public UserController(ApplicationContext context, IWebHostEnvironment webHostEnvironment) {
			db = context;
			hostEnvironment = webHostEnvironment;
		}

		[Authorize]
		public async Task<IActionResult> Profile(int id) {
			User? user = await db.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == id);
			var orders = db.Orders.Include(o => o.Book)
				.Where(o => o.UserId == id && o.Status != "Delivered").GroupBy(o => o.OrderGroupId);
			var groupedOrders = await orders.Select(g => g.ToList()).ToArrayAsync();

			if (user is null)
				return BadRequest();
			ProfileViewModel profileVM = new() {
				User = user,
				Orders = groupedOrders
			};
			return View(profileVM);
		}

		[Authorize]
		public async Task<IActionResult> PurchaseHistory() {
			int userId = int.Parse(User.FindFirst("UserId")?.Value!);
			var orders = db.Orders.Include(o => o.Book).Where(o => o.UserId == userId)
				.Where(o => o.Status == "Delivered" || o.Status == "Cancelled").GroupBy(o => o.OrderGroupId);
			var groupedOrders = await orders.Select(g => g.ToList()).ToArrayAsync();
			return View(groupedOrders);
		}

		[Authorize]
		public async Task<IActionResult> Edit(int id) {
			UserProfile? userProfile = await db.UserProfiles
				.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == id);
			if (userProfile is null)
				return BadRequest();
			return View(userProfile);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([Bind("Name", "Location", "PhoneNumber", "AvatarFile")] UserProfile profile) {
			if (!ModelState.IsValid)
				return View(profile);

			int userId = int.Parse(User.FindFirst("UserId")?.Value!);
			User? user = await db.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == userId);
			if (user is not null) {
				user.Profile!.Name = profile.Name;
				user.Profile!.Location = profile.Location;
				user.Profile!.PhoneNumber = profile.PhoneNumber;
				if (profile.AvatarFile is not null) {
					string wwwRootPath = hostEnvironment.WebRootPath;
					if (user.Profile!.AvatarUrl is not null) {
						string oldAvatarUrl = Path.Combine(wwwRootPath, "assets\\avatars", user.Profile!.AvatarUrl);
						System.IO.File.Delete(oldAvatarUrl);
					}
					string? fileName = Path.GetFileNameWithoutExtension(profile.AvatarFile.FileName);
					string? extension = Path.GetExtension(profile.AvatarFile.FileName);
					user.Profile!.AvatarUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
					string path = Path.Combine(wwwRootPath, "assets\\avatars", fileName);
					using var fileStream = new FileStream(path, FileMode.Create);
					await profile.AvatarFile.CopyToAsync(fileStream);
				}
				db.Users.Update(user);
				await db.SaveChangesAsync();

                return RedirectToAction("Profile", new { id = user.Id });
			}
			else {
				return BadRequest();
			}
		}

		[Authorize]
		public async Task<IActionResult> Cart() {
			string? userId = User.FindFirst("UserId")?.Value;
			if (!string.IsNullOrEmpty(userId)) {
				var cartItems = await db.CartItems.Include(i => i.User).Include(i => i.Book)
					.Where(i => i.UserId == int.Parse(userId)).ToListAsync();
				return View(cartItems);
			}
			return NotFound();
		}

		[Authorize]
		public async Task<IActionResult> Checkout() {
			string? userId = User.FindFirst("UserId")?.Value;
			if (!string.IsNullOrEmpty(userId)) {
				var cartItems = await db.CartItems.Include(i => i.User).Include(i => i.Book)
					.Where(i => i.UserId == int.Parse(userId)).ToListAsync();
				if (cartItems.Any(i => i.Book.Count <= 0))
					return BadRequest();

				var user = await db.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
				if (cartItems.Any() && user is not null) { 
					CheckoutViewModel checkoutVM = new() {
						CartItems = cartItems,
						User = user
					};
					return View(checkoutVM);
				}
			}
			return BadRequest();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Checkout(decimal totalPrice) {
			List<Order> orders = new();
			string? userId = User.FindFirst("UserId")?.Value;
			if (!string.IsNullOrEmpty(userId)) {
				var cartItems = await db.CartItems.Include(i => i.User).Include(i => i.Book)
					.Where(i => i.UserId == int.Parse(userId)).ToListAsync();
				string orderGroupId = Guid.NewGuid().ToString();
				foreach (CartItem item in  cartItems) {
					var order = new Order {
						Book = item.Book,
						BookId = item.BookId,
						User = item.User,
						UserId = item.UserId,
						Total = totalPrice,
						OrderGroupId = orderGroupId,
						Status = "Waiting for dispatch"
					};
					item.Book.Count -= 1;
					if (item.Book.Count < 0)
						item.Book.Count = 0;
					orders.Add(order);
					db.Books.Update(item.Book);
				}
				db.Orders.AddRange(orders);
				db.CartItems.RemoveRange(cartItems);
				db.SaveChanges();
				return RedirectToAction("Profile", "User", new { Id = int.Parse(userId) });
			}
			return BadRequest();
		}
	}
}
