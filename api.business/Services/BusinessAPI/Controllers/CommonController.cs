using BusinessAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Utils.Extensions;
using static BusinessSQLDB.Models.StoredProcedure.commonModels;

namespace Authentication.Controllers
{
    //[Authorize]
    [Route("common")]
    public class CommonController : AppControllerBase
    {

        private readonly ILogger<CommonController> logger;

        private readonly ICommonService _common_Service;


        public CommonController(

            ILogger<CommonController> logger,
            ICommonService common_Service

        )
        {
            this.logger = logger;
            this._common_Service = common_Service;
        }

        [HttpPost]
        [Route("miscellaneous")]
        public async Task<IActionResult> getProductionMiscellaneous([FromBody] sp_Common_GetMiscCombo_Criteria criteria)
        {
            try
            {
                if (criteria == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = await _common_Service.sp_Common_GetMiscCombo(criteria);
                return Ok(results);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }







    }
}
