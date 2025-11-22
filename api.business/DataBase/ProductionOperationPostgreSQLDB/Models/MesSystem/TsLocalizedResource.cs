using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsLocalizedResource
{
    public string ScreenCode { get; set; } = null!;

    public string ObjectId { get; set; } = null!;

    public string? ResourcesEn { get; set; }

    public string? ResourcesTh { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateBy { get; set; }
}
