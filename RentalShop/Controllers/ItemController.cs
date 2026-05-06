using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalShop.Application.Facades;
using RentalShop.Domain.Entities;
using RentalShop.Domain.States;
using RentalShop.Models;

namespace RentalShop.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly RentalShopFacade _facade;

        public ItemController(RentalShopFacade facade) => _facade = facade;

        public async Task<IActionResult> Index()
        {
            var items = await _facade.GetCatalogAsync();
            var vm = items.Select(i => new CatalogItemViewModel
            {
                Sku                = i.Sku,
                Name               = i.Name,
                Category           = i is ToolItem ? "Tool" : "Gear",
                PricePerDay        = i.BasePricePerDay,
                StateName          = ToDisplayStateName(i.CurrentState),
                IsAvailable        = i.CurrentState is AvailableState,
                IsRented           = i.CurrentState is RentedState,
                ExpectedReturnDate = i.ExpectedReturnDate
            }).ToList();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string sku)
        {
            var item = await _facade.GetItemDetailsAsync(sku);
            if (item is null) return NotFound();

            var vm = new ItemDetailsViewModel
            {
                Sku                = item.Sku,
                Name               = item.Name,
                Category           = item is ToolItem ? "Tool" : "Gear",
                PricePerDay        = item.BasePricePerDay,
                StateName          = ToDisplayStateName(item.CurrentState),
                IsAvailable        = item.CurrentState is AvailableState,
                IsRented           = item.CurrentState is RentedState,
                ExpectedReturnDate = item.ExpectedReturnDate,
                Version            = item.Version
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(string sku)
        {
            var ok = await _facade.ProcessReturnAsync(sku);
            if (ok)
                TempData["SuccessMessage"] = "Item returned successfully. Receipt generated.";
            else
                TempData["ErrorMessage"] = $"Could not process return for '{sku}'. It may not be in a rented state.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string sku)
        {
            var ok = await _facade.DeleteCatalogItemAsync(sku);
            if (ok)
                TempData["SuccessMessage"] = "Item deleted from catalog.";
            else
                TempData["ErrorMessage"] = $"Could not delete '{sku}'. Only available items can be deleted.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateItemViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateItemViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _facade.CreateCatalogItemAsync(vm.Name, vm.PricePerDay, vm.Category);

            TempData["SuccessMessage"] = $"{vm.Category:G} item '{vm.Name}' added to catalog.";
            return RedirectToAction(nameof(Index));
        }

        private static string ToDisplayStateName(IItemState state)
            => state.GetType().Name.Replace("State", string.Empty);
    }
}
