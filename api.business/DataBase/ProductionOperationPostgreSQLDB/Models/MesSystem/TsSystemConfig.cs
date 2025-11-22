using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsSystemConfig
{
    public string ConfigCode { get; set; } = null!;

    public string? ConfigDesc { get; set; }

    public string? ValueVarchar { get; set; }

    public string? ValueVarchar2 { get; set; }

    public string? ValueVarchar3 { get; set; }

    public int? ValueInt { get; set; }

    public DateTime? ValueDate { get; set; }

    public decimal? ValueDecimal { get; set; }

    public bool IsActive { get; set; }
}
