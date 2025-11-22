using static WEB.APP.ApiClients.ApiClientsModels.BaseApiClientsModels;
using static WEB.APP.ApiClients.ApiClientsModels.UserManagesGroupPermissionApiClientsModel;

namespace WEB.APP.ApiClients
{
    public partial class ScreenFunction
    {
        public int FunctionId { get; set; }
        public int FunctionCode { get; set; }
        public string FunctionName { get; set; }
    }
    public partial class GroupPermissionDataView
    {
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleCode { get; set; }
        public string SubModuleName { get; set; }
        public int Seq { get; set; }
        public string ScreenId { get; set; }
        public string ScreenName { get; set; }
        public int ScreenFunctionCode { get; set; }
        public int PermissionFunctionCode { get; set; }
    }
    public partial class Module
    {
        public string? ModuleCode { get; set; }
        public string? ModuleName_EN { get; set; }
        public string? ModuleName_TH { get; set; }
        public int Seq { get; set; }
        public string? IconClass { get; set; }
    }
    public partial interface IUserManagesGroupPermissionApiClients
    {
        Task<IEnumerable<ScreenFunction>> ListScreenFunctions();

        Task<IEnumerable<GroupPermissionDataView>> ListByUserGroup(string roleId);
        Task<API_UMS030_UpdatePermission_Result> Update(UMS030_UpdatePermission_Criteria permissions);
        Task<IEnumerable<Module>> ListModules(bool productionLineOnly);
    }



    public class UserManagesGroupPermissionApiClients : IUserManagesGroupPermissionApiClients
    {
        private readonly HttpClient _httpClient;
        public UserManagesGroupPermissionApiClients(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GroupPermissionDataView>> ListByUserGroup(API_UMS030_ListByUserGroup_Criteria criteria)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/ums030/list/user-group", criteria);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<GroupPermissionDataView>>>();

                if (result != null)
                {
                    // result.status = true;
                    return (IEnumerable<GroupPermissionDataView>)result;
                }
            }

            return null;
        }

        public async Task<IEnumerable<GroupPermissionDataView>> ListByUserGroup(string roleId)

        {

            API_UMS030_ListByUserGroup_Criteria criteria = new API_UMS030_ListByUserGroup_Criteria
            {
                userGroupId = roleId,

            };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/ums030/list/user-group", criteria);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<GroupPermissionDataView>>>();

                if (result != null)
                {
                    // result.status = true;
                    return (IEnumerable<GroupPermissionDataView>)result.Data;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Module>> ListModules(bool productionLineOnly)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/system/getmodules", (object)null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<Module>>();

                if (result != null) // ตรวจสอบให้แน่ใจว่า API สำเร็จ
                {
                    return result;
                }
            }

            return Enumerable.Empty<Module>();
        }

        public async Task<IEnumerable<ScreenFunction>> ListScreenFunctions()
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/ums030/list/functions", (object)null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ScreenFunction>>>();

                if (result != null)
                {
                    // result.status = true;
                    return (IEnumerable<ScreenFunction>)result.Data;
                }
            }

            return null;
        }

        public async Task<API_UMS030_UpdatePermission_Result> Update(UMS030_UpdatePermission_Criteria permissions)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/ums030/update/permissions", permissions);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<API_UMS030_UpdatePermission_Result>>();

                if (result != null)
                {
                    // result.status = true;
                    return result.Data;
                }
            }

            return new API_UMS030_UpdatePermission_Result
            {
                StatusCode = "ERROR",
                StatusName = "ไม่สำเร็จ",
                MessageCode = "SERVICE_CREATE_EXCEPTION",
                MessageName = "SERVICE_CREATE_EXCEPTION"
            };
        }
    }
}
