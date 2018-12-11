using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.ViewModel
{
    public class RegisterViewModel
    {
        public int UserId { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [StringLength(30, ErrorMessage = "طول {0} باید کمتر از {1} کاراکتر باشد.")]
        //[Remote("IsUserExist", "Account", HttpMethod = "POST", ErrorMessage = " این نام کاربری  قبلا در سایت ثبت شده است !")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Display(Name = "  نام و نام خانوادگی")]
        [StringLength(40, ErrorMessage = "طول {0} باید کمتر از {1} کاراکتر باشد.")]
        public string FullName { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "طول {0} باید بیشتر از 5 کاراکتر  و کمتر از 30 کاراکتر باشد")]
        public string Password { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن باید یکسان باشند")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "پست الکترونیکی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید"), StringLength(50, MinimumLength = 6, ErrorMessage = "طول {0} باید بیشتر از 5 کاراکتر  و کمتر از 50 کاراکتر باشد."),
        RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
        ErrorMessage = "پست الکترونیکی وارد شده معتبر نمی باشد")]
        //[Remote("IsMailExist", "Account", HttpMethod = "POST", ErrorMessage = "این پست الکترونیکی  قبلا در سایت ثبت شده است !")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "لطفا {0}  را مشخص نمایید")]
        [Display(Name = "جنسیت")]
        public bool Gender { get; set; }


        [Display(Name = " علاقه مندی")]
        public virtual string Favorite { get; set; }
        [Display(Name = " محل تولد")]
        public virtual string BirthPlace { get; set; }
        [Display(Name = " محل سکونت")]
        public virtual string ResidencyPlace { get; set; }
        [Display(Name = " تحصیلات")]
        public virtual string StudyLevel { get; set; }
        [Display(Name = " شغل")]
        public virtual string Job { get; set; }


        [Display(Name = "تصویر کاربر")]
        public string Avatar { get; set; }
        public string ThumbAvatar { get; set; }
        [Display(Name = "تلفن همراه")]
        [RegularExpression(RegexPatterns.Mobile, ErrorMessage = "شماره موبایل وارد شده معتبر نمی باشد")]
        public string Mobile { get; set; }

        [Display(Name = " تاریخ تولد")]
        public DateTime? BirthDate
        {
            get
            {
                if (BirthdayYear == null)
                {
                    return null;
                }
                return new PersianDateTime(BirthdayYear.GetValueOrDefault(), BirthdayMonth.GetValueOrDefault(), BirthdayDay.GetValueOrDefault()).ToDateTime();
            }
        }


        [Display(Name = "سال تولد")]
        public int? BirthdayYear { get; set; }
        [Display(Name = "ماه تولد")]
        public int? BirthdayMonth { get; set; }
        [Display(Name = "روز تولد")]
        public int? BirthdayDay { get; set; }
    }
}
