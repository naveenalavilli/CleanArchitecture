using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.Data.Entities
{
    // TODO: Implement Fluent API 
    public partial class AddressState
    {
        public AddressState()
        {

        }
        [Key]
        public Guid StateId { get; set; }
        public string Name { get; set; }
        public string StateAbbreviation { get; set; }
        public string Country { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
    }
}
