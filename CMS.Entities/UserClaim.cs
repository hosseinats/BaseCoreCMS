using CMS.Entities.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CMS.Entities
{
    public class UserClaim : IdentityUserClaim<int> , IBaseEntity
    {
        public virtual User User { get; set; }
    }
}