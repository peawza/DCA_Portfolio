using BusinessSQLDB;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Utils.SqlServer;
using static BusinessSQLDB.Models.StoredProcedure.commonModels;

namespace BusinessAPI.Repositories
{

    public interface ICommonRepository
    {
        Task<IEnumerable<sp_Common_GetMiscCombo_Result>> sp_Common_GetMiscCombo(sp_Common_GetMiscCombo_Criteria Criteria);

    }

    public class CommonRepository : ICommonRepository
    {

        private MSDBContext _context { get; set; }

        public CommonRepository(MSDBContext context)
        {
            this._context = context;

        }

        public async Task<IEnumerable<sp_Common_GetMiscCombo_Result>> sp_Common_GetMiscCombo(sp_Common_GetMiscCombo_Criteria Criteria)
        {
            var parameters = new SqlParameter[] {
                    SqlParameterHelper.Create("@MiscTypeCode",Criteria.MiscTypeCode),
                     SqlParameterHelper.Create("@Status",Criteria.Status),

            };

            string CallStoredProcedure = SqlParameterHelper.CallStoredProcedure("sp_Common_GetMiscCombo", parameters);
            var result = await _context.Set<sp_Common_GetMiscCombo_Result>().FromSqlRaw(CallStoredProcedure, parameters).ToListAsync();
            return result;
        }

    }
}
