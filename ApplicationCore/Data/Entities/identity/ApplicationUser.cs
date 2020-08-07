using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() : base()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string AvatarURL { get; set; }
        
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
