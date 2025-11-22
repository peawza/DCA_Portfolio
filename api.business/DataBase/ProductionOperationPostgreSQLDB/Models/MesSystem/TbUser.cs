using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbUser
{
    public string Id { get; set; } = null!;

    public bool FirstLoginFlag { get; set; }

    public bool ActiveFlag { get; set; }

    public bool SystemAdminFlag { get; set; }

    public bool Otplogin { get; set; }

    public int? PasswordAge { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? LastUpdatePasswordDate { get; set; }

    public DateTime? ActiveDate { get; set; }

    public DateTime? InActiveDate { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<TbPasswordHistory> TbPasswordHistories { get; set; } = new List<TbPasswordHistory>();

    public virtual ICollection<TbUserClaim> TbUserClaims { get; set; } = new List<TbUserClaim>();

    public virtual ICollection<TbUserLogin> TbUserLogins { get; set; } = new List<TbUserLogin>();

    public virtual ICollection<TbUserRole> TbUserRoles { get; set; } = new List<TbUserRole>();

    public virtual ICollection<TbUserToken> TbUserTokens { get; set; } = new List<TbUserToken>();
}
