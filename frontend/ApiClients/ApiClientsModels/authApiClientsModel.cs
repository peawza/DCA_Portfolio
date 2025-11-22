namespace WEB.APP.ApiClients.ApiClientsModels
{
    public class authApiClientsModel
    {
        public class API_Login_Criteria
        {
            public string AppCode { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            //public bool Remember { get; set; }
            public string VerifyCode { get; set; }
            public string LanguageCode { get; set; }

            public string Device { get; set; }



        }

        public class API_Login_Result
        {
            public bool status { get; set; }
            public string? Id { get; set; }
            public int UserNumber { get; set; }
            public string? UserName { get; set; }
            public string? LanguageCode { get; set; }
            public string? DisplayName { get; set; }
            public string? Token { get; set; }
            public string? RefreshToken { get; set; }
            public double Timeout { get; set; }
            public int RemindPasswordExpired { get; set; }
            public string PermissionScreen { get; set; }
            public string? errorResponse { get; set; }

            public bool? VerifyLogin { get; set; }


        }
        //RefreshTokenCriteria


        public class API_RefreshToken_Criteria
        {
            public string AppCode { get; set; }
            public string UserName { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public string LanguageCode { get; set; }

        }

        public class API_RefreshToken_Result
        {
            public bool status { get; set; }
            public string? Id { get; set; }
            public string? errorResponse { get; set; }
            public string? Token { get; set; }
            public string? RefreshToken { get; set; }
            public double Timeout { get; set; }
        }



        public class MESScreenCriteria
        {
            public string? SupportDeviceType { get; set; }
        }

        public class MESScreenResult
        {
            public string ScreenId { get; set; }
            public string Name_EN { get; set; }
            public string Name_TH { get; set; }
            public int FunctionCode { get; set; }
            public string ModuleCode { get; set; }
            public string ModuleName_EN { get; set; }
            public string ModuleName_TH { get; set; }
            public int ModuleName_Seq { get; set; }
            public string? ModuleName_IconClass { get; set; }
            public string SubModuleCode { get; set; }
            public string SubModuleName_EN { get; set; }
            public string SubModuleName_TH { get; set; }
            public string SubModule_IconClass { get; set; }
            public string Screen_IconClass { get; set; }
            public bool? Screen_MainMenuFlag { get; set; }
            public bool? Screen_PermissionFlag { get; set; }
            public int Screen_Seq { get; set; }
            public string PageTitleName_EN { get; set; }
            public string PageTitleName_TH { get; set; }

        }

        #region GetSiteMaps
        public class API_GetSiteMaps_Criteria
        {
            public string? SupportDeviceType { get; set; }
        }


        public class API_GetSiteMaps_Result
        {
            public List<ModuleModel> Module { get; set; }
            public List<SubModuleModel> SubModule { get; set; }
            public List<GetSiteScreenModel> Screen { get; set; }

        }

        public class ModuleModel
        {
            public string ModuleCode { get; set; }
            public string Module_EN { get; set; }
            public string Module_TH { get; set; }
            public int Seq { get; set; }

            public string IconClass { get; set; }
        }

        public class SubModuleModel
        {
            public string SubModuleCode { get; set; }
            public string SubModuleName_EN { get; set; }
            public string SubModuleName_TH { get; set; }
            public int Seq { get; set; }

            public string IconClass { get; set; }
        }
        public class GetSiteScreenModel
        {
            public string ScreenId { get; set; }
            public string Name_EN { get; set; }
            public string Name_TH { get; set; }
            public int FunctionCode { get; set; }
            public string ModuleCode { get; set; }
            public int ModuleSeq { get; set; }
            public string ModuleName_EN { get; set; }
            public string ModuleName_TH { get; set; }
            public int ModuleName_Seq { get; set; }
            public string? ModuleName_IconClass { get; set; }
            public string SubModuleCode { get; set; }
            public int SubModuleSeq { get; set; }
            public string SubModuleName_EN { get; set; }
            public string SubModuleName_TH { get; set; }
            public string SubModule_IconClass { get; set; }
            public string Screen_IconClass { get; set; }
            public bool Screen_MainMenuFlag { get; set; }
            public bool Screen_PermissionFlag { get; set; }
            public int Screen_Seq { get; set; }
            public string PageTitleName_EN { get; set; }
            public string PageTitleName_TH { get; set; }
            public bool MainMenuFlag { get; set; }
            public bool PermissionFlag { get; set; }
        }

        #endregion

        #region Modules
        public class API_Modules_Criteria
        {

        }

        public class API_Modules_Result
        {
            public int Id { get; set; }
            public string ModuleCode { get; set; }
            public string ModuleName_EN { get; set; }
            public string ModuleName_TH { get; set; }
            public string? IconClass { get; set; }
        }


        #endregion


        #region Roles
        public class API_Roles_Criteria
        {

        }

        public class API_Roles_Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string NormalizedName { get; set; }
            public string Description { get; set; }
        }


        #endregion

        #region ChangePassword

        public class ChangePassword_Criteria
        {
            //public string UserName { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        public class API_ChangePassword_Criteria
        {
            public string UserName { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        public class API_ChangePassword_Result
        {
            public string? StatusCode { get; set; }
            public string? StatusName { get; set; }
            public string? MessageCode { get; set; }
            public string? MessageName { get; set; }
        }

        #endregion




    }
}
