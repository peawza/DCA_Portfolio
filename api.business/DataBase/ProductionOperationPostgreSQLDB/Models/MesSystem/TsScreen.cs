using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsScreen
{
    public string ScreenId { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public string NameTh { get; set; } = null!;

    public string? SupportDeviceType { get; set; }

    public int FunctionCode { get; set; }

    public string ModuleCode { get; set; } = null!;

    public string? SubModuleCode { get; set; }

    public string? IconClass { get; set; }

    public bool MainMenuFlag { get; set; }

    public bool PermissionFlag { get; set; }

    public int Seq { get; set; }

    public string? PageTitleNameEn { get; set; }

    public string? PageTitleNameTh { get; set; }
}
