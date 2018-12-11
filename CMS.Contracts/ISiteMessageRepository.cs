using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace CMS.Contracts
{
    public interface ISiteMessageRepository : IBaseRepository<SiteMessage>
    {
        IQueryable<SiteMessage> AllUserMessages(int userId);
        IEnumerable<SiteMessage> ResponedMessages(int userId);
        IEnumerable<SiteMessage> UnRespondMessages(int userId);
        IEnumerable<SiteMessage> ReadMessages(int userId);
        IEnumerable<SiteMessage> UnReadMessages(int userId);
    }
}
