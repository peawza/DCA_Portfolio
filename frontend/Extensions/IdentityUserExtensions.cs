


using Newtonsoft.Json;
using System.Security.Claims;
using WEB.APP.Extensions.Identity;

namespace WEB.APP.Extensions
{
    public static class IdentityUserExtensions
    {
        public static UserData ToUserData(this ApplicationUser user, Dictionary<string, int[]> permissions, Department department, Position position)
        {
            return new UserData(permissions)
            {
                Id = user.Id,
                UserName = user.UserName,
                //FirstName = user.FirstName,
                //LastName = user.LastName,
                DepartmentNo = user.DepartmentNo,
                DepartmentName = department?.DepartmentName,
                PositionNo = user.PositionNo,
                PositionName = position?.PositionName,
                //Permissions = permissions
            };
        }

        public static UserData GetUserData(this ClaimsPrincipal p)
        {
            //if (!p.HasClaim(t => t.Type == ClaimTypes.UserData))
            //{
            //    return new UserData();
            //}

            //var c = p.FindFirst(ClaimTypes.UserData);
            //return JsonConvert.DeserializeObject<UserData>(c.Value);

            var userData = new UserData();

            var idClaim = p.FindFirst(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(idClaim?.Value))
            {
                userData.Id = idClaim.Value;
            }

            var userName = p.FindFirst("UserName")?.Value;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                userData.UserName = userName;
            }

            //var firstName = p.FindFirst("FirstName")?.Value;
            //if (!string.IsNullOrWhiteSpace(firstName))
            //{
            //    userData.FirstName = firstName;
            //}

            //var lastName = p.FindFirst("LastName")?.Value;
            //if (!string.IsNullOrWhiteSpace(lastName))
            //{
            //    userData.LastName = lastName;
            //}

            var departmentNo = p.FindFirst("DepartmentNo")?.Value;
            if (!string.IsNullOrWhiteSpace(departmentNo))
            {
                userData.DepartmentNo = departmentNo;
            }

            var departmentName = p.FindFirst("DepartmentName")?.Value;
            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                userData.DepartmentName = departmentName;
            }

            var positionNo = p.FindFirst("PositionNo")?.Value;
            if (!string.IsNullOrWhiteSpace(positionNo))
            {
                userData.PositionNo = positionNo;
            }

            var positionName = p.FindFirst("PositionName")?.Value;
            if (!string.IsNullOrWhiteSpace(positionName))
            {
                userData.PositionName = positionName;
            }

            //var team = p.FindFirst("Team")?.Value;
            //if (!string.IsNullOrWhiteSpace(team))
            //{
            //    userData.Team = team;
            //}

            // Optional: If you later store Permissions in a Claim, parse it here (e.g. as JSON)
            var permissionsClaim = p.FindFirst("Permissions")?.Value;
            if (!string.IsNullOrWhiteSpace(permissionsClaim))
            {
                try
                {
                    var permissions = JsonConvert.DeserializeObject<Dictionary<string, int[]>>(permissionsClaim);
                    if (permissions != null)
                    {
                        foreach (var kvp in permissions)
                        {
                            userData.Permissions[kvp.Key] = kvp.Value;
                        }
                    }
                }
                catch
                {
                    // You might want to log this
                }
            }

            return userData;

        }


        public static int[] GetPagePermission(this ClaimsPrincipal principal, string objectId)
        {
            var userData = principal.GetUserData();
            //var functionCode = 0;
            int[] arrayfunctionCode;

            if (String.IsNullOrWhiteSpace(objectId)) { return null; }
            if (userData.Permissions.TryGetValue(objectId, out arrayfunctionCode))
            {
                if (arrayfunctionCode.Length > 0)
                {


                    return arrayfunctionCode;

                }
                else
                {
                    return null;

                }

            }
            return null;
        }

        public static bool HasPermission(this ClaimsPrincipal principal, string objectId, int functionCode = 1)
        {

            if (String.IsNullOrWhiteSpace(objectId)) { return true; }
            var userData = GetUserData(principal);
            var f = 0;
            var values = objectId.Split(',');
            var result = false;
            int[] arrayfunctionCode;
            foreach (var value in values)
            {
                if (userData.Permissions.TryGetValue(value, out arrayfunctionCode))
                {
                    if (arrayfunctionCode.Length > 0)
                    {

                        for (int index = 0; index < arrayfunctionCode.Length; index++)
                        {
                            if (result != true)
                            {
                                result |= (arrayfunctionCode[index] & functionCode) > 0;
                            }
                            continue;
                        }

                    }

                }
            }
            return result;
        }

    }
}
