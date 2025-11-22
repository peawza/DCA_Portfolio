using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsSubModule
{
    public int Id { get; set; }

    public string SubModuleCode { get; set; } = null!;

    public string SubModuleNameEn { get; set; } = null!;

    public string SubModuleNameTh { get; set; } = null!;

    public int Seq { get; set; }

    public string? IconClass { get; set; }
}
