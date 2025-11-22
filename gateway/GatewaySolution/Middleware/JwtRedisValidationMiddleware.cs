
using System.Text.RegularExpressions;

namespace Gateway_Yarp_Solution.Middleware
{
    public class JwtRedisValidationMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly IEncryptionHelper _encryption;
        public JwtRedisValidationMiddleware(RequestDelegate next)
        {
            _next = next;
            //_encryption = encryption;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //var db = redis.GetDatabase();
            var allowAnonymousPaths = new[] { "/api/public/", "/health", "/swagger" };
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            if (allowAnonymousPaths.Any(p => path.StartsWith(p)))
            {
                await _next(context);
                return;
            }




            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }


            await _next(context);
        }


        private static bool IsSemverLike(string v)
        {

            return Regex.IsMatch(v, @"^\d+(\.\d+){0,2}$");
        }


        private static int CompareSemver(string a, string b)
        {
            int[] pa = Parse(a);
            int[] pb = Parse(b);
            for (int i = 0; i < 3; i++)
            {
                int diff = pa[i] - pb[i];
                if (diff != 0) return diff > 0 ? 1 : -1;
            }
            return 0;

            static int[] Parse(string s)
            {
                var parts = s.Split('.', StringSplitOptions.RemoveEmptyEntries);
                var arr = new int[3];
                for (int i = 0; i < arr.Length && i < parts.Length; i++)
                    arr[i] = int.TryParse(parts[i], out var n) ? n : 0;
                return arr;
            }
        }
    }

}
