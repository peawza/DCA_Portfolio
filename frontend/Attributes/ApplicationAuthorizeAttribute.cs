using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WEB.APP.Extensions;


namespace WEB.APP
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApplicationAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public struct NameCodeItem
        {
            public string Name { get; set; }
            public string DispalyName { get; set; }
            public int FunctionCode { get; set; }
        }
        public class PermissionNames
        {
            public static readonly NameCodeItem View = new NameCodeItem { Name = "View", FunctionCode = 1 };
            public static readonly NameCodeItem New = new NameCodeItem { Name = "New", FunctionCode = 2 };
            public static readonly NameCodeItem Edit = new NameCodeItem { Name = "Edit", FunctionCode = 4 };
            public static readonly NameCodeItem Delete = new NameCodeItem { Name = "Delete", FunctionCode = 8 };
            public static readonly NameCodeItem Export = new NameCodeItem { Name = "Export", FunctionCode = 16 };
            public static readonly NameCodeItem Print = new NameCodeItem { Name = "Print", FunctionCode = 32 };
            public static readonly NameCodeItem Special_1 = new NameCodeItem { Name = "Special 1", FunctionCode = 64 };
            public static readonly NameCodeItem Special_2 = new NameCodeItem { Name = "Special 2", FunctionCode = 128 };
            public static readonly NameCodeItem Special_3 = new NameCodeItem { Name = "Special 2", FunctionCode = 256 };
            public static readonly NameCodeItem Import = new NameCodeItem { Name = "Import", FunctionCode = 512 };

            public static Dictionary<string, int> ToDictionary()
            {
                return new Dictionary<string, int>
            {
                { View.Name, View.FunctionCode },
                { New.Name, New.FunctionCode },
                { Edit.Name, Edit.FunctionCode },
                { Delete.Name, Delete.FunctionCode },
                { Export.Name, Export.FunctionCode },
                { Print.Name, Print.FunctionCode },
                { Special_1.Name, Special_1.FunctionCode },
                { Special_2.Name, Special_2.FunctionCode },
                { Special_3.Name, Special_3.FunctionCode },
                { Import.Name, Import.FunctionCode },
            };
            }
        }
        public ApplicationAuthorizeAttribute(string objectId) : this(objectId, PermissionNames.View.FunctionCode)
        {

        }

        public ApplicationAuthorizeAttribute(string objectId, int permission)
        {
            this.ObjectId = objectId;
            this.Permission = permission;
        }

        public string ObjectId { get; private set; }
        public int Permission { get; private set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            var isAjaxRequest = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (!isAuthenticated)
            {
                if (isAjaxRequest)
                {
                    context.Result = new JsonResult(new { message = "Authentication is required." })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                }
                else
                {
                    context.Result = new RedirectToRouteResult("SignIn", new { });
                }
            }

            var isAuthorized = context.HttpContext.User.HasPermission(this.ObjectId, this.Permission);
            if (!isAuthorized)
            {
                if (isAjaxRequest)
                {
                    context.Result = new JsonResult(new { message = "Unauthorized Error. Request resource not allow." })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                }
                else
                {
                    context.Result = new RedirectToRouteResult("Error", new { errorCode = "403" });
                }
            }
        }
    }
}
