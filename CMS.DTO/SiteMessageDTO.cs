using System;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Entities;

namespace CMS.DTO
{
    public class SiteMessageDTO
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public string MessageTitle { get; set; }     
        public DateTime MessageDate { get; set; }
        public bool? IsSeen { get; set; }
        public int? SenderUserId { get; set; }
        [ForeignKey(nameof(SenderUserId))]
        public virtual User SenderUser { get; set; }
        public string ResponsText { get; set; }
        public DateTime? SeenDate { get; set; }
        public DateTime? ResponsDate { get; set; }
        public int? ResponserId { get; set; }
        [ForeignKey(nameof(ResponserId))]
        public virtual User Responser { get; set; }
    }
}