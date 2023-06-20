namespace HokkaidoBookshop.Models.ViewModels {
	public class OrdersViewModel {
		public List<Order>[] Orders { get; set; } = null!;
		public List<Order>? OrdersToUpdate { get; set; }
	}
}
