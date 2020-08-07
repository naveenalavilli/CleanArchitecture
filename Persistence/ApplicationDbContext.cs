using System;
using ApplicationCore.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
                                                        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
                                                        ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressState> AddressState { get; set; }
        public virtual DbSet<EventLog> EventLog { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        @"my connection string",
        //        x => x.UseNetTopologySuite());
        //}

    }
}
