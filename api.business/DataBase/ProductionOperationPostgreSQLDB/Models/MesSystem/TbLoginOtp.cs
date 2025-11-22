using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbLoginOtp
{
    public Guid OtpId { get; set; }

    public int? UserId { get; set; }

    public string LoginIdentifier { get; set; } = null!;

    public string OtpCodeHash { get; set; } = null!;

    public string Channel { get; set; } = null!;

    public string? ReferenceNo { get; set; }

    public DateTime ExpireAt { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string Status { get; set; } = null!;

    public int AttemptCount { get; set; }

    public int MaxAttempt { get; set; }

    public string? RequestIp { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}
