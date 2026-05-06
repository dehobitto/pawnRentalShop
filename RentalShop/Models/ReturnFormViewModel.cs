using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RentalShop.Models
{
    public class ReturnFormViewModel
    {
        [Required]
        [Display(Name = "Item SKU")]
        public string Sku { get; set; } = string.Empty;

        public List<SelectListItem> Items { get; set; } = new();
    }
}
