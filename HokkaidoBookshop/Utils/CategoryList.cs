using System.ComponentModel.DataAnnotations;

namespace HokkaidoBookshop.Utils {
	public enum CategoryList {
		Fiction,
		[Display(Name = "Non-Fiction")]
		NonFiction,
		Education
	}
}
