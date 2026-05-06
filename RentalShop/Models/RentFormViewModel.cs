using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RentalShop.Models
{
    public class RentFormViewModel
    {
        [Required]
        [Display(Name = "Item SKU")]
        public string Sku { get; set; } = string.Empty;

        [Required]
        [Range(RentalConstants.MinRentalDays, RentalConstants.MaxRentalDays,
               ErrorMessage = "Rental period must be between 1 and 365 days.")]
        [Display(Name = "Rental days")]
        public int Days { get; set; } = 1;

        public List<SelectListItem> Items { get; set; } = new();
    }
}
