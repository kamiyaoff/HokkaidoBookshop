using HokkaidoBookshop.Models;
using HokkaidoBookshop.Models.ViewModels;
using HokkaidoBookshop.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.ExceptionServices;

namespace HokkaidoBookshop.Controllers {
	public class BooksController : Controller 
	{
		private readonly ApplicationContext db;
        public BooksController(ApplicationContext context) {
			db = context;
        }

        public async Task<IActionResult> Index(string? category, string? subCategory, 
		string? searchString, int page = 1, SortState sortOrder = SortState.NameAsc) {
			int pageSize = 18;
			
			var books = db.Books.Select(b => b);
			if (!string.IsNullOrEmpty(category))
				books = books.Where(b => b.Category == category);
				ViewBag.Category = category;

			if (books.Any() && !string.IsNullOrEmpty(subCategory)) {
				books = books.Where(b => b.SubCategory == subCategory);
				subCategory = subCategory.Replace("+", " ");
				ViewBag.SubCategory = subCategory;
			}

			if (books.Any() && !string.IsNullOrEmpty(searchString))
				books = books.Where(b => b.Title.Contains(searchString)
				|| b.Author.Contains(searchString));
			
			if (books.Any()) {
				switch (sortOrder) {
					case SortState.NameDesc:
						books = books.OrderByDescending(b => b.Title);
						break;
					case SortState.AuthorAsc:
						books = books.OrderBy(b => b.Author);
						break;
					case SortState.AuthorDesc:
						books = books.OrderByDescending(b => b.Author);
						break;
					case SortState.PriceAsc:
						books = books.OrderBy(b => b.Price);
						break;
					case SortState.PriceDesc:
						books = books.OrderByDescending(b => b.Price);
						break;
					default:
						books = books.OrderBy(b => b.Title);
						break;
				}
				
				var count = await books.CountAsync();
				var items = await books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

				BookIndexViewModel bookIndexVM = new(
					items,
					new PageViewModel(count, page, pageSize),
					new SortViewModel(sortOrder)
				);

				return View(bookIndexVM);
			}
				
			return View(null);
		}

		public async Task<IActionResult> Details(int id) {
			string? userId = User.FindFirst("UserId")?.Value;
			User? user = null;
			if (!string.IsNullOrEmpty(userId))
				user = await db.Users.Include(u => u.Profile).FirstAsync(u => u.Id == int.Parse(userId));
			Book? book = await db.Books.Include(b => b.Reviews).ThenInclude(r => r.User)
				.ThenInclude(u => u.Profile).FirstOrDefaultAsync(b => b.Id == id);
			var reviewsCount = book?.Reviews.Count;
			var positiveReviews = 0;
			var positiveReviewsPercents = 0;
			if (reviewsCount > 0) {
				positiveReviews = book!.Reviews.Where(r => r.Regard == "Positive").Count();
				positiveReviewsPercents = (positiveReviews / reviewsCount) * 100 ?? 0;
			}
			if (book is not null) {
				var similarBooks = await db.Books.Where(b => b.Author == book.Author)
					.OrderByDescending(b => b.CreatedAt).Take(6).ToListAsync();
				BookDetailsViewModel detailsVM = new() {
					Book = book,
					User = user,
					ReviewsCount = reviewsCount,
					PositiveReviews = positiveReviews,
					PositiveReviewsPercents = positiveReviewsPercents,
					SimilarBooks = similarBooks
				};
				return View(detailsVM);
			}
			return NotFound();
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddToCart(
		[Bind("Book", "BookId", "User", "UserId", "Quantity")]CartItem cartItem) {
			db.CartItems.Add(cartItem);
			await db.SaveChangesAsync();
			return RedirectToAction("Cart", "User");
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteFromCart(int id) {
			CartItem? item = db.CartItems.Include(i => i.User).FirstOrDefault(c => c.Id == id);
			string? userId = User.FindFirst("UserId")?.Value;
			if (string.IsNullOrEmpty(userId) || item is null)
				return BadRequest();
			if (int.Parse(userId) == item.User.Id) {
				db.CartItems.Remove(item); 
				await db.SaveChangesAsync();
				return RedirectToAction("Cart", "User");
			}
			return BadRequest();
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddComment([Bind("UserId, BookId, Regard, Body")] Review review) {
			db.Reviews.Add(review);
			await db.SaveChangesAsync();
			return RedirectToAction("Details", new { id = review.BookId});
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteComment(int id, int bookId) {
			Review review = new() { Id = id };
			db.Entry(review).State = EntityState.Deleted;
			await db.SaveChangesAsync();
			return RedirectToAction("Details", new { id = bookId });
		}
	}
}
