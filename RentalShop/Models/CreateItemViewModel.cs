using System.ComponentModel.DataAnnotations;
using RentalShop.Domain.Entities;

namespace RentalShop.Models
{
    public class CreateItemViewModel
    {
        [Required, MaxLength(200)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(RentalConstants.MinItemPrice, RentalConstants.MaxItemPrice,
               ErrorMessage = "Price must be between €0.01 and €9 999.99")]
        [Display(Name = "Price per day (€)")]
        public decimal PricePerDay { get; set; }

        [Required]
        [Display(Name = "Category")]
        public ItemCategory Category { get; set; } = ItemCategory.Tool;
    }
}
