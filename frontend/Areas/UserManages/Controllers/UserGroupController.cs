using Microsoft.AspNetCore.Mvc;

namespace WEB.APP.Controllers
{
    //[Authorize]
    //[AllowAnonymous]
    [Area("UserManages")]
    public class UserGroupController : AppControllerBase
    {
        private readonly ILogger<UserGroupController> _logger;
        //private readonly IStringLocalizer _localizer;
        //private readonly IMemoryCache _cache;

        public UserGroupController(ILogger<UserGroupController> logger)
        {
            _logger = logger;
            //_localizer = localizer;
            //_cache = cache;

        }
        [SiteMapTitle(ObjectId = "UMS020")]
        //[AllowAnonymous]
        //[ApplicationAuthorize("UMS020")]
        [Route("~/usermanagement/ums020", Name = "UMS020")]
        public IActionResult ums020_inquiry()
        {
            return View();
        }

    }
}