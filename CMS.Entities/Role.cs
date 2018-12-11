using CMS.Entities.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CMS.Entities
{
    public class Role : IdentityRole<int> , IBaseEntity
    {

        public string Description { get; set; }
        public virtual ICollection<UserRole> Users { get; set; }
        public virtual ICollection<RoleClaim> Claims { get; set; }
    }
}
