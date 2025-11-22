using System;
using System.Collections.Generic;

namespace TempMachinePostgreSQLDB.Models.TempMachine;

public partial class TempProcessWipSummary
{
    public string MachineCode { get; set; } = null!;

    public string ShiftId { get; set; } = null!;

    public int WorkOrderId { get; set; }

    public string? WorkOrderNo { get; set; }

    public string WorkOrderType { get; set; } = null!;

    public int WipLabelId { get; set; }

    public string? WipLabelNo { get; set; }

    public DateTime? WipStartTime { get; set; }

    public DateTime? WipEndTime { get; set; }

    public int? WipStatus { get; set; }

    public int? WipIdleTime { get; set; }

    public int? WipSetupTime { get; set; }

    public int? WipBreakTime { get; set; }

    public int? WipSetupDelayTime { get; set; }

    public int? WipBreakDelayTime { get; set; }

    public int? WipDownTime { get; set; }

    public int? WipRunningTime { get; set; }

    public decimal? StandardCycleTime { get; set; }

    public int? PlanQty { get; set; }

    public bool? IsCountActual { get; set; }

    public int? WipActualQty { get; set; }

    public int? WipOkQty { get; set; }

    public int? WipNgQty { get; set; }

    public decimal? WipEfficiencyTarget { get; set; }

    public decimal? WipOeeAvailability { get; set; }

    public decimal? WipOeePerformance { get; set; }

    public decimal? WipOeeQuality { get; set; }

    public decimal? WipOeeTotal { get; set; }

    public string? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public string? ServerProcessName { get; set; }

    public DateTime? StampTime { get; set; }
}
