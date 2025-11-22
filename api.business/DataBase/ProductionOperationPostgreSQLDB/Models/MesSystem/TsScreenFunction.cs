using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsScreenFunction
{
    public int FunctionId { get; set; }

    public int FunctionCode { get; set; }

    public string FunctionName { get; set; } = null!;
}
