using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbUserInfo
{
    /// <summary>
    /// User ID (Main Key)
    /// </summary>
    public string Id { get; set; } = null!;

    public int UserNumber { get; set; }

    /// <summary>
    /// UserName
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Employee Code
    /// </summary>
    public string? EmployeeCode { get; set; }

    /// <summary>
    /// First Name
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last Name
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Remark, more note.
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Language Code login
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Department Code login
    /// </summary>
    public string? DepartmentCode { get; set; }

    /// <summary>
    /// Position Code login
    /// </summary>
    public string? PositionCode { get; set; }

    /// <summary>
    /// Create Date
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Create By
    /// </summary>
    public string CreatedBy { get; set; } = null!;

    /// <summary>
    /// Update Date
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    /// <summary>
    /// Update By
    /// </summary>
    public string? UpdatedBy { get; set; }

    public bool DeletedFlag { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string? DeletedBy { get; set; }
}
