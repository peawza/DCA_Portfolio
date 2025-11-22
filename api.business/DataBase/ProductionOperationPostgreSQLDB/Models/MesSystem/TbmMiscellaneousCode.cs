using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbmMiscellaneousCode
{
    public string MiscTypeCode { get; set; } = null!;

    public string MiscCode { get; set; } = null!;

    public string MiscName { get; set; } = null!;

    public string? Value1 { get; set; }

    public string? Value2 { get; set; }

    public int SeqNo { get; set; }

    public string? Description { get; set; }

    public bool EditFlag { get; set; }

    public bool DeleteFlag { get; set; }

    public bool ActiveStatus { get; set; }

    public DateTime? InactiveDateTime { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public string UpdateBy { get; set; } = null!;
}
