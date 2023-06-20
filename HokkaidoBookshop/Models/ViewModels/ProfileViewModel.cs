﻿namespace HokkaidoBookshop.Models.ViewModels {
	public class ProfileViewModel {
		public User User { get; set; } = null!;
		public IEnumerable<Order>[] Orders { get; set; } = null!;
	}
}
