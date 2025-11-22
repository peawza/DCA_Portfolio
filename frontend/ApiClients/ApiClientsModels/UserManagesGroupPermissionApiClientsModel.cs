namespace WEB.APP.ApiClients.ApiClientsModels
{
    public class UserManagesGroupPermissionApiClientsModel
    {
        public class API_UMS030_ListByUserGroup_Criteria
        {
            public string userGroupId { get; set; }

        }


        public partial class UMS030_UpdatePermission_Criteria
        {
            public string GroupId { get; set; }
            public string UpdateBy { get; set; }
            public IEnumerable<API_UMS030_UpdatePermission_list_Criteria> GroupPermissionData { get; set; }
        }
        public class API_UMS030_UpdatePermission_list_Criteria
        {
            public string GroupId { get; set; }
            public string ScreenId { get; set; }
            public int FunctionCode { get; set; }
            public DateTime CreateDate { get; set; }
            public string CreateBy { get; set; }
            public DateTime UpdateDate { get; set; }
            public string UpdateBy { get; set; }
        }
        public class API_UMS030_UpdatePermission_Result
        {
            public string? StatusCode { get; set; }
            public string? StatusName { get; set; }
            public string? MessageCode { get; set; }
            public string? MessageName { get; set; }
        }
    }
}
