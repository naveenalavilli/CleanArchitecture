using ApplicationCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.ViewModels
{
    public class StateViewModel
    {
        [Key]
        public Guid StateId { get; set; }
        public AddressState AddressState
        {
            set; get;
        }
        public string CreatedBy
        {
            get; set;
        }
        public string Country
        {
            get; set;
        }

    }
}
