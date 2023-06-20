using Microsoft.EntityFrameworkCore;

namespace HokkaidoBookshop.Models {
	public class ApplicationContext : DbContext {
		public DbSet<Book> Books { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Review> Reviews { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {
		}
	}
}
