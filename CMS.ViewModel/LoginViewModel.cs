using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [StringLength(30, ErrorMessage = "طول {0} باید کمتر از {1} کاراکتر باشد.")]
        public string UserName { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MinLength(7 , ErrorMessage = "حداقل 7 کارکتر برای کلمه عبور الزامی است")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string returnUrl { get; set; }
    }
}
