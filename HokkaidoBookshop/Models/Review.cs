﻿using System.Linq.Expressions;

namespace HokkaidoBookshop.Models {
	public class Review {
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; } = null!;
		public int BookId { get; set; }
		public Book Book { get; set; } = null!;
		public string Regard { get; set; } = null!;
		public string? Body { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
}
