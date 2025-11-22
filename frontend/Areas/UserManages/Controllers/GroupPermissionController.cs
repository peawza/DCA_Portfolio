using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using WEB.APP.ApiClients;
using WEB.APP.Controllers;
using WEB.APP.Extensions;
using WEB.APP.Extensions.Identity;
using static WEB.APP.ApiClients.ApiClientsModels.UserManagesGroupPermissionApiClientsModel;
using static WEB.APP.Areas.UserManages.Models.UMS030;

namespace WEB.APP.Areas.UserManages.Controllers
{
    //[Authorize]
    [Area("UserManages")]
    public class GroupPermissionController : AppControllerBase
    {
        private readonly ILogger<GroupPermissionController> _logger;
        private readonly IUserManagesGroupPermissionApiClients _service;
        private readonly IauthApiClients _auth_service;


        public GroupPermissionController(ILogger<GroupPermissionController> logger, IUserManagesGroupPermissionApiClients service, IauthApiClients auth_service)
        {
            _logger = logger;
            _service = service;
            _auth_service = auth_service;
        }
        [SiteMapTitle(ObjectId = "UMS030")]
        //[ApplicationAuthorize("UMS020")]
        //[AllowAnonymous]
        [Route("~/UserManages/GroupPermission/Index", Name = "UMS030")]
        public async Task<IActionResult> Index(string? id)
        {
            //return View();
            var screenFunctions = await _service.ListScreenFunctions();

            ViewBag.Id = id;
            return View(new PermissionInquiryViewModel()
            {
                ScreenFunctions = screenFunctions.ToDictionary(t => t.FunctionName, k => k.FunctionCode),
            });
        }

        [HttpPost]
        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string userGroupId)
        {
            try
            {
                var screenFunctions = (await _service.ListScreenFunctions()).ToDictionary(t => t.FunctionName.Replace(" ", ""), k => k.FunctionCode);
                var dataItems = (await _service.ListByUserGroup(userGroupId)) ?? Enumerable.Empty<GroupPermissionDataView>();
                var models = dataItems.Select(data => ToViewModel(data, screenFunctions)).ToList();
                var result = models.ToDataSourceResult(request);
                return JsonWithDefaultOptions(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Read User Group Permission error.");
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(GroupPermissionData data, string userGroupId)
        {
            var UserData = User.GetUserData();
            var dataItem = ToEntity(data, UserData.UserName, userGroupId);

            //dataItem
            var result = await _service.Update(new UMS030_UpdatePermission_Criteria
            {
                GroupId = userGroupId,
                UpdateBy = UserData.UserName,
                GroupPermissionData = dataItem
            });
            if (result.StatusCode == "SUCCESS")
            {
                return Ok("Group permission save success.");
            }
            return InternalServerError("Save User Group Permission error.");
        }
        //[Route("/group/permission/modules/list", Name = "ListModules")]
        //public async Task<IActionResult> ListModules()
        //{
        //    var models = await _service.ListModules(false);
        //    return JsonWithDefaultOptions(models);
        //}
        //[Route("/group/permission/roles/list", Name = "ListRoles")]
        //public async Task<IActionResult> ListRoles()
        //{
        //    var models = await _auth_service.getRoles();
        //    return JsonWithDefaultOptions(models);
        //}



        public static GroupPermissionViewModel ToViewModel(GroupPermissionDataView entity, Dictionary<string, int> permissions)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));
            var model = new GroupPermissionViewModel
            {
                Id = Guid.NewGuid(), // สร้างใหม่ทุกครั้ง
                ModuleCode = entity.ModuleCode,
                ModuleName = entity.ModuleName,
                SubModuleCode = entity != null ? entity.SubModuleCode : "##",
                SubModuleName = entity != null ? entity.SubModuleName : "##",
                Seq = entity.Seq,
                ScreenId = entity.ScreenId,
                ScreenName = entity.ScreenName,
                ScreenFunctionCode = entity.ScreenFunctionCode,
                Permissions = new Dictionary<string, bool>()
            };
            model.ScreenName = entity.ScreenName;
            foreach (var key in permissions.Keys)
            {
                model.Permissions.Add(key, (permissions[key] & entity.PermissionFunctionCode) > 0);
            }
            return model;
        }
        public partial class GroupPermission
        {
            public string GroupId { get; set; }
            public string ScreenId { get; set; }
            public int FunctionCode { get; set; }
            public DateTime CreateDate { get; set; }
            public string CreateBy { get; set; }
            public DateTime UpdateDate { get; set; }
            public string UpdateBy { get; set; }
        }
        public static IEnumerable<API_UMS030_UpdatePermission_list_Criteria> ToEntity(GroupPermissionData data, string user, string userGroupId)
        {
            var permissions = new List<API_UMS030_UpdatePermission_list_Criteria>();
            var screens = data.Permissions.Select(t => t.ScreenId).Distinct();
            foreach (var screen in screens)
            {
                var v = 0;
                data.Permissions.Where(t => t.ScreenId == screen).ToList().ForEach((f) => v |= f.FunctionCode);

                permissions.Add(new API_UMS030_UpdatePermission_list_Criteria() { ScreenId = screen, FunctionCode = v, GroupId = userGroupId, CreateBy = user, UpdateBy = user });
            }
            return permissions;
        }
    }
}
