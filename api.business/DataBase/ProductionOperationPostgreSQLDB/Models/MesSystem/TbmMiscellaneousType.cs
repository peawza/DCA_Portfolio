using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbmMiscellaneousType
{
    public string MiscTypeCode { get; set; } = null!;

    public string MiscTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool AddFlag { get; set; }

    public bool ModifyFlag { get; set; }

    public bool DeleteFlag { get; set; }

    public bool ActiveStatus { get; set; }
}
