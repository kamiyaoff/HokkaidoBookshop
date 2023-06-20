using HokkaidoBookshop.Models;
using HokkaidoBookshop.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace HokkaidoBookshop.Controllers {
    public class HomeController : Controller 
    {
        private readonly ApplicationContext db;
        const string authScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        public HomeController(ApplicationContext context) {
            db = context;
        }

        public async Task<IActionResult> Index() {
            var books = db.Books.Include(b => b.Reviews);
            var bestBooks = await books.OrderByDescending(b => b.Reviews.Count).Take(6).ToListAsync();
            var newBooks = await books.OrderByDescending(b => b.CreatedAt).Take(6).ToListAsync();
            var cheapBooks = await books.OrderBy(b => b.Price).Take(6).ToListAsync();
            HomeViewModel homeVM = new() {
                BestBooks = bestBooks,
                NewBooks = newBooks,
                CheapBooks = cheapBooks
            };
            return View(homeVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login() {
            if (User.Identity is not null) {
				if (User.Identity.IsAuthenticated)
					return RedirectToAction("Index", "Home");
			}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email", "Password")] User inputData, string? ReturnUrl) {
            User? user = await db.Users.FirstOrDefaultAsync(u => 
                u.Email == inputData.Email && u.Password == inputData.Password);
            if (user is null)
                return View(inputData);

            var claims = new List<Claim> {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var claimsIdentity = new ClaimsIdentity(claims, authScheme);
            await HttpContext.SignInAsync(authScheme, new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect(ReturnUrl ?? "/");
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(authScheme);
            return LocalRedirect("/");
        }

		public IActionResult Signup() {
			if (User.Identity is not null) {
				if (User.Identity.IsAuthenticated)
					return RedirectToAction("Index", "Home");
			}
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Email", "Password", "ConfirmPassword", "TermsConditions")] User user) {
            if (!ModelState.IsValid)
				return View(user);

            if (db.Users.Any(u => u.Email == user.Email)) {
                ModelState.AddModelError("Email", "Email address is already in use.");
                return View(user);
            }

            user.Profile = new UserProfile();
            user.Role = "User";
            db.Users.Add(user);
            await db.SaveChangesAsync();
			var claims = new List<Claim> {
				new Claim("UserId", user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
			};
			var claimsIdentity = new ClaimsIdentity(claims, authScheme);
			await HttpContext.SignInAsync(authScheme, new ClaimsPrincipal(claimsIdentity));

			return LocalRedirect("/");
		}
	}
}