using ApplicationCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Observers
{
    public interface IStateObserver
    {
        void Update(AddressState State);
    }
}
