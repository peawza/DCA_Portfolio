namespace Authentication.Models
{
    public class CommonModels
    {



        #region Common_Department
        public class Common_Department_Criteria
        {
            // ยังไม่ใช้เงื่อนไข (future use)
        }

        public class Common_Department_Result
        {
            public int Id { get; set; }
            public string DepartmentCode { get; set; }
            public string DepartmentName { get; set; }
        }
        #endregion


        #region Common_Position
        public class Common_Position_Criteria
        {
            // ยังไม่ใช้เงื่อนไข (future use)
        }

        public class Common_Position_Result
        {
            public int Id { get; set; }
            public string PositionCode { get; set; }
            public string PositionName { get; set; }
        }
        #endregion


        #region  Common_Role
        public class Common_Role_Criteria
        {
            // ยังไม่ใช้เงื่อนไข (future use)
        }

        public class Common_Role_Result
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string NormalizedName { get; set; }
            public string? Description { get; set; }
            public bool IsActive { get; set; }
        }
        #endregion


        #region Common_MiscellaneousModules
        public class Common_MiscellaneousModules_Criteria
        {
            // ยังไม่ใช้เงื่อนไข (future use)
        }

        public class Common_MiscellaneousModules_Result
        {
            public string ModulesCode { get; set; }
            public string ModulesName { get; set; }
            public string ModulesNameDisplay { get; set; }
        }


        public class Common_MiscellaneousModulesApi_Result : Common_MiscellaneousModules_Result
        {

            public string ConnectionDB { get; set; }

        }

        //Common_MiscellaneousModules_Criteria
        #endregion
    }
}
