using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsUrlconfig
{
    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Remark { get; set; }

    public string? UpdateUserId { get; set; }

    public DateTime? UpdateDateTime { get; set; }
}
