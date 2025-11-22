using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsImportLogDetail
{
    public Guid Id { get; set; }

    public string JobLogId { get; set; } = null!;

    public int RowNo { get; set; }

    public string ErrorDetail { get; set; } = null!;
}
