using ApplicationCore.Data;
using ApplicationCore.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using services.contracts;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services
{
   public class AddressStateService : IAddressStateService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private readonly ILogger _logger;

        public AddressStateService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, ILogger<AddressStateService> logger)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public async Task<AddressState> GetAddress(Guid AddressStateId)
        {
           // StateViewModel stateViewModel = new StateViewModel();
            var addressState = await _context.AddressState.Include(c => c.UpdatedBy).FirstOrDefaultAsync(m => m.StateId == AddressStateId);
            if (addressState == null)
            {
                return null;
            }
            return addressState;
            //stateViewModel.StateId = addressState.StateId;
            //stateViewModel.AddressState = addressState;
            //stateViewModel.Country = "USA";
            //stateViewModel.CreatedBy = addressState.UpdatedBy.Email;
            //return stateViewModel;
        }

        public async Task<List<StateViewModel >> GetAllAddresses()
        {
            List<StateViewModel> statesList = new List<StateViewModel>();
            var AddressStateList = await _context.AddressState.Include(c => c.UpdatedBy).ToListAsync();

            foreach(var addressState in AddressStateList)
            {
                statesList.Add(new StateViewModel() {StateId= addressState.StateId, AddressState = addressState, CreatedBy = addressState.UpdatedBy.Email, Country = "USA" });
            }
            return statesList;
        }

        public async Task<AddressState> SetAddress(AddressState AddressState)
        {
            AddressState.CreatedBy = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            AddressState.UpdatedBy = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            AddressState.CreatedOn = DateTime.UtcNow;
            AddressState.UpdatedOn = DateTime.UtcNow;
            AddressState.StateId = new Guid();

            _context.AddressState.Add(AddressState);
            await _context.SaveChangesAsync();

            return AddressState;
        }

        public async Task<AddressState> UpdateAddress(AddressState AddressState)
        {
            AddressState.UpdatedBy = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            AddressState.UpdatedOn = DateTime.UtcNow;

            _context.Update(AddressState);
            await _context.SaveChangesAsync();

            return AddressState;
        }

        public async Task<bool> DeleteAddressState(Guid AddressStateId)
        {
            var addressState = await _context.AddressState.FindAsync(AddressStateId).ConfigureAwait(true);
            _context.AddressState.Remove(addressState);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool IsAddressStateExist(Guid AddressStateId)
        {
            return  _context.AddressState.Any(e => e.StateId == AddressStateId);
        }

        public async Task<bool> DeleteAddressStateByAppUserId(Guid AppUserId)
        {
            var addressStates = await _context.AddressState.Include(x => x.CreatedBy).Where(x => x.CreatedBy.Id == AppUserId).ToListAsync().ConfigureAwait(true);
           
            foreach(var state in addressStates)
            {
                _context.AddressState.Remove(state);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}