namespace WEB.APP.Extensions.Identity
{
    public class UserData
    {
        public UserData()
        {

        }

        public UserData(Dictionary<string, int[]> permissions)
        {
            Permissions = permissions;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeCode { get; set; }
        public string DisplayName { get; set; }
        public string DepartmentNo { get; set; }
        public string DepartmentName { get; set; }
        public string PositionNo { get; set; }
        public string PositionName { get; set; }

        public string LanguageCode { get; set; }

        public Dictionary<string, int[]> Permissions { get; private set; } = new Dictionary<string, int[]>();

    }
}
