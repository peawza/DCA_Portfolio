using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsModule
{
    public int Id { get; set; }

    public string ModuleCode { get; set; } = null!;

    public string ModuleNameEn { get; set; } = null!;

    public string ModuleNameTh { get; set; } = null!;

    public int Seq { get; set; }

    public string? IconClass { get; set; }
}
