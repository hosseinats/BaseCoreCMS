using Microsoft.AspNetCore.Identity;

namespace CMS.Entities
{
    public class UserToken : IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}
