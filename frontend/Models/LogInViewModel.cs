using System.ComponentModel.DataAnnotations;

namespace WEB.APP.Models
{
    public class LoginViewModel
    {
        //[Required]
        [Display(Name = "UserName")]
        public string? UserName { get; set; }
        //[Required]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        public string? hiddenLanguage { get; set; }
        public string? returnUrl { get; set; }
        //[Display(Name = "Company")]
        //public string Company { get; set; }
    }

    public class EncodeLoginViewModel
    {

        public string? hiddenUserName { get; set; }


        public string? hiddenPassword { get; set; }

        //[Required]
        //public string __RequestVerificationToken { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
        public string? returnUrl { get; set; }
    }


    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}
