using System.ComponentModel.DataAnnotations;

namespace WEB.APP.Areas.UserManages.Models
{
    public class UMS030
    {
        public class GroupPermissionViewModel
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string ModuleCode { get; set; }
            public string ModuleName { get; set; }
            public string SubModuleCode { get; set; }
            [Display(Name = "Sub Module")]
            public string SubModuleName { get; set; }
            public int Seq { get; set; }
            public string ScreenId { get; set; }
            public string ScreenName { get; set; }
            public Dictionary<string, bool> Permissions { get; set; } = new Dictionary<string, bool>();
            public int ScreenFunctionCode { get; set; }

            public string GroupName => $"{this.Seq:00}_{ModuleName}";
            [Display(Name = "")]
            public string SubGroupName => $"{this.Seq:00}_{this.ModuleCode}_{SubModuleName?.Replace("Sub", "")}";
        }


        public class PermissionInquiryViewModel
        {
            public Dictionary<string, int> ScreenFunctions = new Dictionary<string, int>();
        }

        public class GroupPermissionData
        {
            public int UserGroupId { get; set; }
            public IList<GroupPermissionDataItem> Permissions { get; set; } = new List<GroupPermissionDataItem>();
        }
        public class GroupPermissionDataItem
        {
            public string ScreenId { get; set; }
            public int FunctionCode { get; set; }
        }
    }
}
