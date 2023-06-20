using Microsoft.AspNetCore.Mvc;
using HokkaidoBookshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Caching.Memory;
using HokkaidoBookshop.Models.ViewModels;

namespace HokkaidoBookshop.Controllers {
	public class AdminController : Controller 
	{
		private readonly ApplicationContext db;
		private readonly IWebHostEnvironment hostEnvironment;

        public AdminController(ApplicationContext context, IWebHostEnvironment webHostEnvironment) {
			db = context;
			hostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin")]
		public IActionResult AddBook() {
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddBook(Book book) {
			if (!ModelState.IsValid)
				return View(book);

			if (book.PosterFile is not null) {
				string wwwRootPath = hostEnvironment.WebRootPath;
				string? fileName = Path.GetFileNameWithoutExtension(book.PosterFile.FileName);
				string? extension = Path.GetExtension(book.PosterFile.FileName);
				book.PosterUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
				string path = Path.Combine(wwwRootPath, "assets\\posters", fileName);
				using var fileStream = new FileStream(path, FileMode.Create);
				await book.PosterFile.CopyToAsync(fileStream);
			}
			db.Books.Add(book);
			await db.SaveChangesAsync();
			return RedirectToAction("Details", "Books", new { id = book.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteBook(int id) {
			Book? book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
			if (book is not null) {
				if (book.PosterUrl is not null) {
					string wwwRootPath = hostEnvironment.WebRootPath;
					string oldAvatarUrl = Path.Combine(wwwRootPath, "assets\\posters", book.PosterUrl);
					System.IO.File.Delete(oldAvatarUrl);
				}
				db.Books.Remove(book);
				await db.SaveChangesAsync();
				return RedirectToAction("Index", "Home");
			}
			return NotFound();
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditBook(int id) {
			Book? book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
			if (book is not null)
				return View(book);
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditBook(int id, [Bind("Id", "Title", "Author", "Publisher", 
		"Price", "Count", "Category", "SubCategory", "PosterUrl", "PosterFile", "Description")] Book book) {
			if (id != book.Id)
                return NotFound();
			if (!ModelState.IsValid)
				return View(book);

			if (book.PosterFile != null && book.PosterFile.Length > 0 && !string.IsNullOrEmpty(book.PosterFile.FileName)) {
				string wwwRootPath = hostEnvironment.WebRootPath;
				if (book.PosterUrl is not null) {
					string oldAvatarUrl = Path.Combine(wwwRootPath, "assets\\posters", book.PosterUrl);
					System.IO.File.Delete(oldAvatarUrl);
				}
				string? fileName = Path.GetFileNameWithoutExtension(book.PosterFile.FileName);
				string? extension = Path.GetExtension(book.PosterFile.FileName);
				book.PosterUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
				string path = Path.Combine(wwwRootPath, "assets\\posters", fileName);
				using var fileStream = new FileStream(path, FileMode.Create);
				await book.PosterFile.CopyToAsync(fileStream);
			}
			db.Books.Update(book);
			await db.SaveChangesAsync();
			return RedirectToAction("Details", "Books", new { id = book.Id });
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Orders() {
			var orders = db.Orders.Include(o => o.Book).GroupBy(o => o.OrderGroupId)
				.OrderBy(o => o.First().Status).OrderByDescending(o => o.First().CreatedAt);
			var groupedOrders = await orders.Select(g => g.ToList()).ToArrayAsync();
			OrdersViewModel ordersVM = new() {
				Orders = groupedOrders
			};
			return View(ordersVM);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateOrder(string groupId, string newStatus) {
			var orders = db.Orders.Where(o => o.OrderGroupId == groupId);
			foreach (var order in orders) {
				order.Status = newStatus;
				order.UpdatedAt = DateTime.Now;
			}
			db.Orders.UpdateRange(orders);
			await db.SaveChangesAsync();
			return RedirectToAction("Orders", "Admin");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteOrder(string groupId) {
			var orders = db.Orders.Where(o => o.OrderGroupId == groupId);
			if (!orders.Any())
				return BadRequest();
			db.Orders.RemoveRange(orders);
			await db.SaveChangesAsync();
			return RedirectToAction("Orders", "Admin");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteUser(int id) {
			User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user is null)
				return NotFound();
			db.Users.Remove(user);
			await db.SaveChangesAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
