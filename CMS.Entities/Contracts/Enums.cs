using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMS.Entities.Contracts
{
   public class Enums
    {
        public enum PostType
        {
            [Display(Name = "پست بلاگ")]
            BlogPost = 1,
            [Display(Name = "اخبار")]
            News,
            [Display(Name = "ویکی")]
            Wiki, 
            [Display(Name = "ویدئو پست")]
            VieoPost
        }
        public enum CommentStatus
        {
            /* 0 - approved, 1 - pending, 2 - spam, -1 - trash */
            [Display(Name = "تأیید شده")]
            Approved = 0,
            [Display(Name = "در انتظار بررسی")]
            Pending = 1,
            [Display(Name = "عدم تأیید")]
            Spam = 2,
            [Display(Name = "پاک شده")]
            Trash = -1
        }
        public enum Language
        {
            [Display(Name = "فارسی")]
            Persian ,
            English
        }

        public enum Socials
        {
            Telegram =1,
            Instagram ,
            Whatsapp,
            Twitter,
            Facebook,
            LinkedIn,
            Youtube,
            Aparat
        }
    }
}
