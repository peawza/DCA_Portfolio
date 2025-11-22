using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.APP.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [Area("UserManages")]
    public class MiscellaneousController : AppControllerBase
    {
        private readonly ILogger<MiscellaneousController> _logger;
        //private readonly IStringLocalizer _localizer;
        //private readonly IMemoryCache _cache;

        public MiscellaneousController(ILogger<MiscellaneousController> logger)
        {
            _logger = logger;
            //_localizer = localizer;
            //_cache = cache;

        }
        [SiteMapTitle(ObjectId = "CMS040")]
        [AllowAnonymous]
        //[ApplicationAuthorize("CMS040")]
        [Route("~/usermanagement/cms040", Name = "CMS040")]
        public IActionResult cms040_inquiry()
        {
            return View();
        }

    }
}