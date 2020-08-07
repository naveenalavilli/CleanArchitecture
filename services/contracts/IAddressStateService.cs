﻿using ApplicationCore.Data.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace services.contracts
{
    public interface IAddressStateService
    {
        bool IsAddressStateExist(Guid AddressStateId);

         Task<List<StateViewModel>> GetAllAddresses();
         Task<AddressState> GetAddress(Guid AddressStateId);

        Task<AddressState> SetAddress(AddressState AddressState);

        Task<AddressState> UpdateAddress(AddressState AddressState);

        Task<bool> DeleteAddressState(Guid AddressStateId);
        Task<bool> DeleteAddressStateByAppUserId(Guid AppUserId);

    }
}
