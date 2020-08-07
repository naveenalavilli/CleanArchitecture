using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Data;
using ApplicationCore.Data.Entities;
using services;
using services.contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace web.Controllers
{
    [Authorize]
    public class AddressStatesController : Controller
    {
        private readonly IAddressStateService _addressStateService;
        private readonly UserManager<ApplicationUser> _userManager;


        public AddressStatesController(UserManager<ApplicationUser> userManager, IAddressStateService addressStateService)
        {
            _userManager = userManager;
            _addressStateService = addressStateService;
        }

        // GET: AddressStates
        public async Task<IActionResult> Index()
        {
            return View(await _addressStateService.GetAllAddresses());
        }

        // GET: AddressStates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(await _addressStateService.GetAddress(id.Value));
        }

        // GET: AddressStates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddressStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StateId,Name,StateAbbreviation,CreatedOn,UpdatedOn")] AddressState addressState)
        {
            if (ModelState.IsValid)
            {
                await _addressStateService.SetAddress(addressState);
                return RedirectToAction(nameof(Index));
            }
            return View(addressState);
        }

        // GET: AddressStates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressState = await _addressStateService.GetAddress(id.Value);
            if (addressState == null)
            {
                return NotFound();
            }
            return View(addressState);
        }

        // POST: AddressStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("StateId,Name,StateAbbreviation,CreatedOn,UpdatedOn")] AddressState addressState)
        {
            if (id != addressState.StateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _addressStateService.UpdateAddress(addressState);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressStateExists(addressState.StateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(addressState);
        }

        // GET: AddressStates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressState = await _addressStateService.GetAddress(id.Value);
            if (addressState == null)
            {
                return NotFound();
            }

            return View(addressState);
        }

        // POST: AddressStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
           await _addressStateService.DeleteAddressState(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AddressStateExists(Guid id)
        {
            return _addressStateService.IsAddressStateExist(id);
        }
    }
}
