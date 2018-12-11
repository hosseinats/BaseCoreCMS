using CMS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.DataLayer.Context;
using CMS.Entities;

namespace CMS.Services
{

    public class SiteMessageRepository : BaseRepository<SiteMessage>, ISiteMessageRepository
    {
        public SiteMessageRepository(CMSContext cmsContext) : base(cmsContext)
        {
        }

        public IQueryable<SiteMessage> AllUserMessages(int userId)
        {
            return GetAll().Where(p => p.SenderUserId == userId);
        }

        public IEnumerable<SiteMessage> ResponedMessages(int userId)
        {
            return AllUserMessages(userId).Where(p => p.ResponsText != null).ToList();
        }
        public IEnumerable<SiteMessage> UnRespondMessages(int userId)
        {
            return AllUserMessages(userId).Where(p => p.ResponsText == null).ToList();
        }
        public IEnumerable<SiteMessage> ReadMessages(int userId)
        {
            return AllUserMessages(userId).Where(p => p.IsSeen == true).ToList();
        }
        public IEnumerable<SiteMessage> UnReadMessages(int userId)
        {
            return AllUserMessages(userId).Where(p => p.IsSeen != true).ToList();
        }
    }
}
