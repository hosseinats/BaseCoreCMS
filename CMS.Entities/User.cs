using CMS.Entities.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
   public class User : IdentityUser<int> , IBaseEntity
    {

        [StringLength(40)]
        public string FullName { get; set; }
        public bool Gender { get; set; } = true;
        public string Avatar { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? LastVisitDateTime { get; set; }
        public string Location { set; get; }
        public bool IsBaned { get; set; } = false;
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserToken> UserTokens { get; set; }
        public ICollection<UserLogin> Logins { get; set; }

        [InverseProperty("SenderUser")]
        public ICollection<SiteMessage> SenderUserMessages { get; set; }
        [InverseProperty("Responser")]
        public ICollection<SiteMessage> ResponserMessages { get; set; }

    }
}

