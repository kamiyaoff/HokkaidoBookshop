using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HokkaidoBookshop.Models {
	public static class SeedData {
		public static void Initialize(IServiceProvider serviceProvider) {
			using var context = new ApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>());
			if (context.Books.Any())
				return;

			context.Books.AddRange(
				new Book {
					Title = "Dark Souls",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "Dark Souls 3",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 79.99M,
					Count = 421,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "The Witcher: 3",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "Sekiro: Shadows Die Twice",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "Diablo IV",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "Pathologic 2",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "Half-Life",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "Silent Hill",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "0",
					SubCategory = "Romance"
				},
				new Book {
					Title = "Mortal Kombat X",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "Divinity: Original Sin",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "Tekken 7",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "Dota 2",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "Cyberpunk 2077",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				}, 
				new Book {
					Title = "Disco Elysium",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "The Life and Suffering of Sir Brante",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "Fallout: New Vegas",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "Pathfinder: Kingmaker",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "1",
					SubCategory = "Art, Creative"
				},
				new Book {
					Title = "Doom",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "2",
					SubCategory = "Sciences"
				},
				new Book {
					Title = "Ori and The Blind Forest",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "2",
					SubCategory = "Sciences"
				},
				new Book {
					Title = "Portal",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "2",
					SubCategory = "Sciences"
				},
				new Book {
					Title = "Fran Bow",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Category = "2",
					SubCategory = "Sciences"
				},
				new Book {
					Title = "Amnesia: The Dark Descent",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "2",
					SubCategory = "Sciences"
				},
				new Book {
					Title = "SOMA",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "2",
					SubCategory = "Sciences"
				},
				new Book {
					Title = "Resident Evil 2",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "2",
					SubCategory = "Sciences"
				},
				new Book {
					Title = "Hotline Miami",
					Author = "Hidetaki Miyazaki",
					Publisher = "FromSoftware",
					Description = "Just a game where you die several times",
					Price = 59.99M,
					Count = 142,
					Category = "2",
					SubCategory = "Sciences"
				}
			);

			context.Users.AddRange(
				new User {
					Email = "anna.malenok@mail.ru",
					Password = "malenok",
					Role = "User",
					Profile = new UserProfile {
						Name = "Anna Malenok",
						Location = "Kazakhstan, Aktobe",
						PhoneNumber = "1234567890"
					}
				},
				new User {
					Email = "admin@gmail.ru",
					Password = "myadmin",
					Role = "Admin",
					Profile = new UserProfile()
				}
			);

			context.SaveChanges();
		}
	}
}
