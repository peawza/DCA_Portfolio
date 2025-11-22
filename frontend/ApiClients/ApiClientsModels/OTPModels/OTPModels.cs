using Microsoft.Build.Framework;

namespace WEB.APP.ApiClients.ApiClientsModels.OTPModels
{
    public class OTPModels
    {

        public class SendOtpRequest
        {
            public string UserName { get; set; } = null!;
            public string Channel { get; set; } = "EMAIL";   // SMS / EMAIL / APP
        }

        public class SendOtpResponse
        {
            public Guid Token { get; set; }                  // ใช้เป็น Token (OtpId)
            public DateTime ExpireAt { get; set; }

            public string ReferenceNo { get; set; } = "";
            public string DestinationMask { get; set; } = "";
        }

        public class ResendOtpRequest
        {
            [Required]
            public Guid Token { get; set; }                  // OtpId เดิม
        }

        public class ResendOtpResponse
        {
            public Guid Token { get; set; }
            public DateTime ExpireAt { get; set; }
        }

        public class VerifyOtpRequest
        {
            [Required]
            public Guid Token { get; set; }
            [Required]
            public string OtpCode { get; set; } = null!;
            public string Device { get; set; } = null!;

        }

        public class VerifyOtpResponse
        {
            public string? StatusCode { get; set; }
            public string? StatusName { get; set; }
            public string? MessageCode { get; set; }
            public string? MessageName { get; set; }
            public bool Verified { get; set; }
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

        }
    }
}
