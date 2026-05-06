using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentalShop.Application.Facades;
using RentalShop.Domain.Entities;
using RentalShop.Models;

namespace RentalShop.Controllers
{
    [Authorize]
    public class RentalController : Controller
    {
        private readonly RentalShopFacade _facade;

        public RentalController(RentalShopFacade facade) => _facade = facade;

        [HttpGet]
        public async Task<IActionResult> Rent(string? sku)
        {
            var vm = new RentFormViewModel { Items = await LoadItemSelectListAsync() };
            if (sku is not null) vm.Sku = sku;
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent(RentFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                var ok = await _facade.ProcessRentalAsync(form.Sku, form.Days);
                if (ok)
                {
                    TempData["SuccessMessage"] = $"Item rented for {form.Days} day(s). Contract generated.";
                    return RedirectToAction("Index", "Item");
                }
                TempData["ErrorMessage"] = "Sorry, this item is no longer available or was modified by someone else.";
                return RedirectToAction("Index", "Item");
            }
            form.Items = await LoadItemSelectListAsync();
            return View(form);
        }

        [HttpGet]
        public async Task<IActionResult> Return(string? sku)
        {
            var vm = new ReturnFormViewModel { Items = await LoadItemSelectListAsync() };
            if (sku is not null) vm.Sku = sku;
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(ReturnFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                var ok = await _facade.ProcessReturnAsync(form.Sku);
                if (ok)
                {
                    TempData["SuccessMessage"] = $"Item returned successfully. Receipt generated.";
                    return RedirectToAction("Index", "Item");
                }
                TempData["ErrorMessage"] = "Could not process return. The item may not be in a rented state.";
                return RedirectToAction("Index", "Item");
            }
            form.Items = await LoadItemSelectListAsync();
            return View(form);
        }

        private async Task<List<SelectListItem>> LoadItemSelectListAsync()
        {
            var items = await _facade.GetCatalogAsync();
            return items.Select(i => new SelectListItem(ItemSelectLabel(i), i.Sku)).ToList();
        }

        private static string ItemSelectLabel(RentalItem i)
            => $"{i.Sku} — {i.Name} (€{i.BasePricePerDay}/day)";
    }
}
