using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace HokkaidoBookshop.Utils {
	public class CheckBoxRequiredAttribute : ValidationAttribute, IClientModelValidator {

		public override bool IsValid(object? value) {
			if (value is bool v)
				return v;
			return false;
		}

		public void AddValidation(ClientModelValidationContext context) {
			context.Attributes.Add("data-val-checkboxrequired", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
		}
	}
}
