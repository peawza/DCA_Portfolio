using Microsoft.AspNetCore.Identity;

namespace WEB.APP.Extensions.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string EmployeeCode { get; set; }
        public string DisplayName { get; set; }
        public string DepartmentNo { get; set; }
        public string PositionNo { get; set; }

        public string LanguageCode { get; set; }


    }
}
