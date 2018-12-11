using CMS.Entities.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CMS.Entities
{
    public class RoleClaim : IdentityRoleClaim<int> , IBaseEntity
    {
        public virtual Role Role { get; set; }
    }
}