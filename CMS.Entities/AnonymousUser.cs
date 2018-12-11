using System;
using System.Collections.Generic;
using System.Text;
using CMS.Entities.Contracts;

namespace CMS.Entities
{
    public class AnonymousUser : IBaseEntity
    {
        public  int Id { get; set; }
        public  string Name { get; set; }
        public  string Email { get; set; }
        public  string Mobile { get; set; }
        public  string IP { get; set; }
        public virtual ICollection<SiteMessage> SiteMessages { get; set; }
    }
}
