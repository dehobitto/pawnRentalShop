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
    /// <summary>
    /// Thin controller for the rent / return flows. Delegates all business
    /// logic to <see cref="RentalShopFacade"/>; the controller only maps HTTP
    /// to method calls and ViewModels to views.
    /// Requires authentication — unauthenticated requests are redirected to
    /// <c>/Account/Login</c> by the cookie middleware.
    /// </summary>
    [Authorize]
    public class RentalController : Controller
    {
        private readonly RentalShopFacade _facade;

        public RentalController(RentalShopFacade facade) => _facade = facade;

        // ── Rent ─────────────────────────────────────────────────────────────

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
                    TempData["Success"] = $"Item {form.Sku} rented for {form.Days} day(s). Contract generated.";
                    return RedirectToAction("Index", "Item");
                }
                ModelState.AddModelError(nameof(form.Sku), $"Item '{form.Sku}' was not found in the catalog.");
            }
            form.Items = await LoadItemSelectListAsync();
            return View(form);
        }

        // ── Return ────────────────────────────────────────────────────────────

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
                    TempData["Success"] = $"Item {form.Sku} returned successfully. Receipt generated.";
                    return RedirectToAction("Index", "Item");
                }
                ModelState.AddModelError(nameof(form.Sku), $"Item '{form.Sku}' was not found in the catalog.");
            }
            form.Items = await LoadItemSelectListAsync();
            return View(form);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private async Task<List<SelectListItem>> LoadItemSelectListAsync()
        {
            var items = await _facade.GetCatalogAsync();
            return items.Select(i => new SelectListItem(ItemSelectLabel(i), i.Sku)).ToList();
        }

        private static string ItemSelectLabel(RentalItem i)
            => $"{i.Sku} — {i.Name} (€{i.BasePricePerDay}/day)";
    }
}
