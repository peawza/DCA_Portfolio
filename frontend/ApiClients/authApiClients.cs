using System.Net.Http.Headers;
using static WEB.APP.ApiClients.ApiClientsModels.authApiClientsModel;
using static WEB.APP.ApiClients.ApiClientsModels.BaseApiClientsModels;
using static WEB.APP.ApiClients.ApiClientsModels.OTPModels.OTPModels;

namespace WEB.APP.ApiClients
{
    public partial interface IauthApiClients
    {
        Task<API_Login_Result?> API_Login(API_Login_Criteria criteria);
        Task<API_GetSiteMaps_Result?> GetSiteMaps();
        Task<API_RefreshToken_Result?> API_refresh_token(API_RefreshToken_Criteria criteria);
        Task<List<API_Modules_Result>?> getModules();

        Task<List<API_Roles_Result>?> getRoles();


        Task<SendOtpResponse> LoginSendOtp(SendOtpRequest Request);
        Task<ResendOtpResponse> LoginResendOtp(ResendOtpRequest Request);
        Task<VerifyOtpResponse> LoginVerifyOtp(VerifyOtpRequest Request);
        // Task<API_ChangePassword_Result> ChangePassword(API_ChangePassword_Criteria criteria);
    }



    public class authApiClients : IauthApiClients
    {
        private readonly HttpClient _httpClient;
        public authApiClients(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<API_GetSiteMaps_Result?> GetSiteMaps()
        {
            API_GetSiteMaps_Criteria criteria = new API_GetSiteMaps_Criteria()
            {
                SupportDeviceType = "WEB"
            };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/system/getsitemaps", criteria);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<API_GetSiteMaps_Result>();

                if (result != null)
                {
                    //result.status = true;
                    return result;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401 Handling
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("Error 401: Unauthorized access. Please check your credentials.");
                return new API_GetSiteMaps_Result
                {

                };
            }


            throw new Exception("");
        }

        public async Task<API_Login_Result?> API_Login(API_Login_Criteria criteria)
        {
            var relativePath = "/api/auth/SSS010/Login"; // ต้องตรงกับ UpstreamPathTemplate
            using var response = await _httpClient.PostAsJsonAsync(relativePath, criteria);

            var body = await response.Content.ReadAsStringAsync(); // เก็บไว้ดูทุกกรณี
            //var response = await _httpClient.PostAsJsonAsync("/SSS010/Login", criteria);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<API_Login_Result>();

                if (result != null)
                {
                    result.status = true;
                    return result;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401 Handling
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("Error 401: Unauthorized access. Please check your credentials.");
                return new API_Login_Result
                {
                    status = false,
                    errorResponse = errorResponse
                };
            }

            throw new Exception(
                    $"Login failed: {(int)response.StatusCode} {response.StatusCode}. " +
                    $"Base={_httpClient.BaseAddress}, Path={relativePath}, Body={body}");
        }

        public async Task<API_RefreshToken_Result?> API_refresh_token(API_RefreshToken_Criteria criteria)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", criteria.Token);
            var response = await _httpClient.PostAsJsonAsync("/api/auth/SSS010/RefreshToken", criteria);
            //var response = await _httpClient.PostAsJsonAsync("/SSS010/Login", criteria);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<API_RefreshToken_Result>();

                if (result != null)
                {
                    result.status = true;
                    return result;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401 Handling
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("Error 401: Unauthorized access. Please check your credentials.");
                return new API_RefreshToken_Result
                {
                    status = false,
                    errorResponse = errorResponse
                };
            }
            throw new Exception("refresh token error");
        }

        public async Task<List<API_Modules_Result>?> getModules()
        {
            API_Modules_Criteria criteria = new API_Modules_Criteria()
            {
                //SupportDeviceType = "WEB"
            };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/system/getmodules", criteria);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<API_Modules_Result>>();

                if (result != null)
                {
                    //result.status = true;
                    return result;
                }
            }


            return null;
        }

        public async Task<List<API_Roles_Result>?> getRoles()
        {


            API_Roles_Criteria api_Roles_Criteria = new API_Roles_Criteria()
            {
                //SupportDeviceType = "WEB"
            };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/system/getrole", api_Roles_Criteria);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<API_Roles_Result>>();

                if (result != null)
                {
                    //result.status = true;
                    return result;
                }
            }


            return null;
        }



        public async Task<SendOtpResponse> LoginSendOtp(SendOtpRequest Request)
        {



            var response = await _httpClient.PostAsJsonAsync("/api/auth/otp/send-otp", Request);

            if (response.IsSuccessStatusCode)
            {
                // 
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<SendOtpResponse>>();

                if (result != null)
                {
                    //result.status = true;
                    return result.Data;
                }
            }

            throw new Exception("Login SendOtp error");
        }




        public async Task<ResendOtpResponse> LoginResendOtp(ResendOtpRequest Request)
        {



            var response = await _httpClient.PostAsJsonAsync("/api/auth/otp/resend-otp", Request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ResendOtpResponse>>();

                if (result != null)
                {
                    //result.status = true;
                    return result.Data;
                }
            }

            throw new Exception("Login ResendOtp error");
        }

        public async Task<VerifyOtpResponse> LoginVerifyOtp(VerifyOtpRequest Request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/SSS010/otpverifylogin", Request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<VerifyOtpResponse>();

                if (result != null)
                { //
                    result.Verified = true; return result;
                }
            }
            else if (response.StatusCode.ToString() == "BadRequest")
            {
                var result = await response.Content.ReadFromJsonAsync<VerifyOtpResponse>();

                if (result != null)
                { //
                    result.Verified = false;

                    return result;
                }


            }
            //var result = await response.Content.ReadFromJsonAsync<VerifyOtpResponse>();

            throw new Exception("Login VerifyOtp error");
        }


    }
}
