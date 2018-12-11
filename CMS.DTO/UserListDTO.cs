using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.DTO
{
    public class UserListDTO
    {

        public string FullName { get; set; }
        public bool Gender { get; set; } = true;
        public string Avatar { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? LastVisitDateTime { get; set; }
        public string Location { set; get; }
        public bool IsBaned { get; set; } = false;
    }
}
