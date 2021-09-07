using ApplicationCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Observers
{
   public interface IStateNotifier
    {
        // Attach an order observer to the subject.
        void Attach(IStateObserver observer);

        // Detach an order observer from the subject.
        void Detach(IStateObserver observer);

        // Notify all order observers about an event.
        void Notify(AddressState State);
    }
}
