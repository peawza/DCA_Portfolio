using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbPosition
{
    public int Id { get; set; }

    public string PositionCode { get; set; } = null!;

    public string PositionName { get; set; } = null!;
}
