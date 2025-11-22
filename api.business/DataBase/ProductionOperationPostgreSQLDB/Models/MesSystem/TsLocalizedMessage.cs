using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsLocalizedMessage
{
    public string MessageCode { get; set; } = null!;

    public string MessageType { get; set; } = null!;

    public string? MessageNameEn { get; set; }

    public string? MessageNameTh { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateBy { get; set; }
}
