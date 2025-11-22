using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TsImportLog
{
    public string JobLogId { get; set; } = null!;

    public string InterfaceCode { get; set; } = null!;

    public string? InterfaceName { get; set; }

    public string? FtpServername { get; set; }

    public string? FileFolder { get; set; }

    public string InterfaceFilename { get; set; } = null!;

    public string JobStatus { get; set; } = null!;

    public string ProcessBy { get; set; } = null!;

    public DateTime StartDatetime { get; set; }

    public DateTime FinishDatetime { get; set; }

    public int TotalRecord { get; set; }

    public string? Remark { get; set; }
}
