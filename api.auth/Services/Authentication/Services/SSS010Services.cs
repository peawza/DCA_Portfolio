using Application;
using Authentication.Repositories;
using static Authentication.Models.SSS010.SSS010;

namespace Authentication.Services
{
    public partial interface ISSS010Services
    {

        Task<Dictionary<string, int[]>> GetGroupPermissionUser(SSS010_GetGroupPermissionUser_Criteria criteria);



    }
    public class SSS010Services : ISSS010Services
    {
        private readonly ISSS010Repository _repository;



        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SSS010Services(ISSS010Repository repository, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _db = db;

            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<Dictionary<string, int[]>> GetGroupPermissionUser(SSS010_GetGroupPermissionUser_Criteria criteria)
        {
            try
            {

                var GetGroupPermission = await _repository.GetGroupPermissionUser(criteria);
                var permissions = new Dictionary<string, int[]>();
                foreach (var Data in GetGroupPermission)
                {
                    try
                    {
                        if (permissions.ContainsKey(Data.ScreenID))
                        {
                            permissions[Data.ScreenID] = permissions[Data.ScreenID].Concat(new int[] { Data.FunctionCode }).ToArray();
                        }
                        else
                        {
                            int[] FunctionCode = { Data.FunctionCode };
                            permissions.Add(Data.ScreenID, FunctionCode);
                        }

                    }
                    catch (System.Exception ex)
                    {

                        // throw;
                    }

                }



                return permissions;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
