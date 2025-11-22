using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEB.APP.ApiClients;
using WEB.APP.Controllers;
using WEB.APP.Extensions.Identity;
using WEB.APP.Localization;
using WEB.APP.Models;
using static WEB.APP.ApiClients.ApiClientsModels.authApiClientsModel;
using static WEB.APP.ApiClients.ApiClientsModels.OTPModels.OTPModels;

namespace WEB.APP.Areas.Controllers
{
    public class AccountController : AppControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMessageLocalizer _messageLocalizer;
        private readonly IauthApiClients _authApiClients;


        public AccountController(ILogger<AccountController> logger,
            IMessageLocalizer messageLocalizer,
            IauthApiClients authApiClients




        )
        {
            _logger = logger;
            _messageLocalizer = messageLocalizer;
            _authApiClients = authApiClients;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FirstLogin()
        {
            await EnusreSignOut();
            LoginViewModel viewModel = new LoginViewModel();
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn()
        {
            await EnusreSignOut();
            LoginViewModel viewModel = new LoginViewModel();
            return View(viewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp()
        {
            await EnusreSignOut();
            LoginViewModel viewModel = new LoginViewModel();
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost("/verify/otp")]
        public async Task<IActionResult> LoginVerifyOtp([FromBody] VerifyOtpRequest Request)
        {
            try
            {
                Request.Device = "web";
                var VerifyOtp_Data = await _authApiClients.LoginVerifyOtp(Request);
                if (VerifyOtp_Data.Verified == true)
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        Id = VerifyOtp_Data.Id,
                        UserName = VerifyOtp_Data.UserName,
                        DisplayName = VerifyOtp_Data.DisplayName,
                        LanguageCode = VerifyOtp_Data.LanguageCode,
                    };
                    string jsonPermissionScreen = Encoding.UTF8.GetString(Convert.FromBase64String(VerifyOtp_Data.PermissionScreen));
                    Dictionary<string, int[]> Permissions =
                        System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int[]>>(jsonPermissionScreen);
                    var claims = new List<Claim>
                    {
                        //new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.NameIdentifier, VerifyOtp_Data.Id),
                        new Claim("UserName",VerifyOtp_Data.UserName),
                        new Claim("DisplayName", VerifyOtp_Data.DisplayName),
                        new Claim("LanguageCode", VerifyOtp_Data.LanguageCode),
                        new Claim("Token", VerifyOtp_Data.Token),
                        new Claim("RefreshToken", VerifyOtp_Data.RefreshToken),
                        new Claim("Timeout", VerifyOtp_Data.Timeout.ToString()),
                        new Claim("RemindPasswordExpired",VerifyOtp_Data.RemindPasswordExpired.ToString()),
                        new Claim("Permissions",JsonConvert.SerializeObject(Permissions)),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    var isHttps = HttpContext.Request.IsHttps;
                    var expiresAt = DateTimeOffset.UtcNow.AddMinutes(VerifyOtp_Data.Timeout);
                    Response.Cookies.Append("uacj-tms-access-token", VerifyOtp_Data.Token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = isHttps,
                        SameSite = SameSiteMode.Strict,
                        Expires = expiresAt
                    });

                    Response.Cookies.Append("uacj-tms-refresh-token", VerifyOtp_Data.RefreshToken, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = isHttps,
                        SameSite = SameSiteMode.Strict,
                        Expires = expiresAt.AddDays(1)
                    });

                    return Ok(new
                    {
                        staus = true

                    });


                }


                return Ok(new
                {
                    staus = false,
                    StatusCode = VerifyOtp_Data.StatusCode,
                    StatusName = VerifyOtp_Data.StatusName,
                    MessageCode = VerifyOtp_Data.MessageCode,
                    MessageName = VerifyOtp_Data.MessageName
                });


            }
            catch (Exception)
            {

                throw;
            }
        }



        private string FindCommonSubstring(string str1, string str2)
        {
            // Simple implementation to find the longest common substring
            int maxLength = 0;
            string longestCommonSubstring = string.Empty;

            int[,] lengths = new int[str1.Length, str2.Length];

            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] == str2[j])
                    {
                        if (i == 0 || j == 0)
                        {
                            lengths[i, j] = 1;
                        }
                        else
                        {
                            lengths[i, j] = lengths[i - 1, j - 1] + 1;
                        }

                        if (lengths[i, j] > maxLength)
                        {
                            maxLength = lengths[i, j];
                            longestCommonSubstring = str1.Substring(i - maxLength + 1, maxLength);
                        }
                    }
                }
            }

            return longestCommonSubstring;
        }
        public static string DecryptStringAES(string cipherText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            keyBytes = SHA256.HashData(keyBytes); // ทำให้เป็น 256-bit key

            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(fullCipher, 0, fullCipher.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginViewModel viewModel)
        {

            string secretKey = "MESIFY_SECRET_KEY";

            //viewModel.UserName = DecryptStringAES(viewModel.UserName, secretKey);
            //viewModel.Password = DecryptStringAES(viewModel.Password, secretKey);
            //string commonSubstring = FindCommonSubstring(EncodeViewModel.hiddenUserName, EncodeViewModel.hiddenPassword);
            //string decode_UserName = EncodeViewModel.hiddenUserName.Replace(commonSubstring, string.Empty);
            //string decode_Password = EncodeViewModel.hiddenPassword.Replace(commonSubstring, string.Empty);
            //byte[] base64DecodedBytes_UserName = Convert.FromBase64String(decode_UserName);
            //byte[] base64DecodedBytes_Password = Convert.FromBase64String(decode_Password);


            // Ensure we have a valid viewModel to work with
            //LoginViewModel viewModel = new LoginViewModel()
            //{
            //    UserName = Encoding.UTF8.GetString(base64DecodedBytes_UserName).Replace("|||", string.Empty),
            //    Password = Encoding.UTF8.GetString(base64DecodedBytes_Password),
            //    returnUrl = EncodeViewModel.returnUrl
            //};
            if (string.IsNullOrWhiteSpace(viewModel.UserName) || string.IsNullOrWhiteSpace(viewModel.Password))
            {

                var error = _messageLocalizer["EM0005"].Value;
                ViewBag.Error = error;
                return View(viewModel);

            }
            if (!ModelState.IsValid)
            {

                var error = _messageLocalizer["EM0005"].Value;
                ViewBag.Error = error;
                return View(viewModel);

            }

            try
            {
                await EnusreSignOut();

                API_Login_Criteria _Login_criteria = new API_Login_Criteria()
                {
                    AppCode = "MES",
                    UserName = viewModel.UserName,
                    Password = viewModel.Password,
                    //LanguageCode = viewModel.hiddenLanguage,
                    Device = "web"
                };
                var login = await _authApiClients.API_Login(_Login_criteria);
                if (login.status == true && login.VerifyLogin == false)
                {
                    // Update LastSignedIn field.



                    ApplicationUser user = new ApplicationUser()
                    {
                        Id = login.Id,
                        UserName = login.UserName,
                        DisplayName = login.DisplayName,
                        LanguageCode = login.LanguageCode,



                    };
                    string jsonPermissionScreen = Encoding.UTF8.GetString(Convert.FromBase64String(login.PermissionScreen));
                    Dictionary<string, int[]> Permissions =
                        System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int[]>>(jsonPermissionScreen);
                    var claims = new List<Claim>
                    {
                        //new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.NameIdentifier, login.Id),
                        new Claim("UserName",login.UserName),
                        new Claim("DisplayName", login.DisplayName),
                        new Claim("LanguageCode", login.LanguageCode),
                        new Claim("Token", login.Token),
                        new Claim("RefreshToken", login.RefreshToken),
                        new Claim("Timeout", login.Timeout.ToString()),
                        new Claim("RemindPasswordExpired",login.RemindPasswordExpired.ToString()),
                        new Claim("Permissions",JsonConvert.SerializeObject(Permissions)),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    var isHttps = HttpContext.Request.IsHttps;
                    var expiresAt = DateTimeOffset.UtcNow.AddMinutes(login.Timeout);
                    Response.Cookies.Append("uacj-tms-access-token", login.Token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = isHttps,
                        SameSite = SameSiteMode.Strict,
                        Expires = expiresAt
                    });

                    Response.Cookies.Append("uacj-tms-refresh-token", login.RefreshToken, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = isHttps,
                        SameSite = SameSiteMode.Strict,
                        Expires = expiresAt.AddDays(1)
                    });


                    return RedirectToAction("Index", "Home");
                }
                else if (login.status == true && login.VerifyLogin == true)
                {


                    var dataOtp = await _authApiClients.LoginSendOtp(new SendOtpRequest
                    {

                        UserName = viewModel.UserName,
                        Channel = "EMAIL",
                    });



                    var url = Url.Action("VerifyOtp", "Account", new
                    {
                        token = dataOtp.Token,            // Guid -> auto ToString()
                        username = viewModel.UserName,
                        destination = dataOtp.DestinationMask,  // เช่น email mask
                        otp_reference_no = dataOtp.ReferenceNo


                    });

                    return Redirect(url);
                    //return RedirectToAction("VerifyOtp", "Account");
                }
                else
                {
                    var error = _messageLocalizer["EM0006"].Value;
                    ViewBag.Error = error;
                    _logger.LogError(error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sign in error.");
                ViewBag.Error = "EM0006";
            }
            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }


        [Authorize]
        [HttpPost("/refresh/token")]
        public async Task<IActionResult> RefreshToken([FromBody] API_RefreshToken_Criteria request)
        {
            try
            {
                request.UserName = User.FindFirst("UserName")?.Value;
                var RefreshToken = await _authApiClients.API_refresh_token(request);



                var currentClaims = User.Claims.ToList();

                // ลบ Claims เดิมที่ต้องการอัปเดต
                currentClaims.RemoveAll(c => c.Type == "LanguageCode" || c.Type == "Token");

                // เพิ่ม Claims ใหม่
                currentClaims.Add(new Claim("LanguageCode", request.LanguageCode));
                currentClaims.Add(new Claim("Token", RefreshToken.Token));

                // สร้าง ClaimsIdentity ใหม่
                var newIdentity = new ClaimsIdentity(currentClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var newPrincipal = new ClaimsPrincipal(newIdentity);

                // SignIn ใหม่เพื่ออัปเดต
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);
                var isHttps = HttpContext.Request.IsHttps;
                var expiresAt = DateTimeOffset.UtcNow.AddMinutes(RefreshToken.Timeout);
                Response.Cookies.Append("uacj-tms-access-token", RefreshToken.Token, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = isHttps,
                    SameSite = SameSiteMode.Strict,
                    Expires = expiresAt
                });

                Response.Cookies.Append("uacj-tms-refresh-token", RefreshToken.RefreshToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = isHttps,
                    SameSite = SameSiteMode.Strict,
                    Expires = expiresAt.AddDays(1)
                });

                return Ok(new
                {
                    status = "OK",
                });
            }
            catch (Exception ex)
            {

                try
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
                catch (Exception exs)
                {
                    return InternalServerError(new
                    {
                        status = "error",
                        Messages = exs.Message,
                        MessagesDetails = exs.TargetSite,
                    });

                }


                return InternalServerError(new
                {
                    status = "error",
                    Messages = ex.Message,
                    MessagesDetails = ex.TargetSite,
                });
            }

        }
        //[HttpPost("/change/password")]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePassword_Criteria request)
        //{
        //    try
        //    {
        //        API_ChangePassword_Criteria api = new API_ChangePassword_Criteria();

        //        api.UserName = User.FindFirst("UserName")?.Value;
        //        api.OldPassword = request.OldPassword;
        //        api.NewPassword = request.NewPassword;


        //        var result = await _authApiClients.ChangePassword(api);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        return InternalServerError(new
        //        {
        //            status = "error",
        //            Messages = ex.Message,
        //            MessagesDetails = ex.TargetSite,
        //        });
        //    }

        //}


        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            if (!String.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await EnusreSignOut();
            return RedirectToAction("Index", "Home");
        }

        private async Task EnusreSignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Index", "Home");
                //HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
                //await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //return RedirectToAction("SignIn", "Account");
            }
        }


    }
}
