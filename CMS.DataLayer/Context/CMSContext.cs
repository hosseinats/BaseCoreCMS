
using CMS.DataLayer.Mappings;
using CMS.Entities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.DataLayer.Context
{
    public class CMSContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public CMSContext(DbContextOptions<CMSContext> options) : base(options)
        {
        }

        public DbSet<AnonymousUser> AnonymousUsers { get; set; }
        public DbSet<SiteMessage> SiteMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.AddCustomIdentityMappings();

        }
    }
}
