using System;
using System.Collections.Generic;

namespace TempMachinePostgreSQLDB.Models.TempMachine;

public partial class TempProcessMachineSummary
{
    public string MachineCode { get; set; } = null!;

    public string ShiftId { get; set; } = null!;

    public int WorkOrderId { get; set; }

    public string? WorkOrderNo { get; set; }

    public int WipLabelId { get; set; }

    public string? WipLabelNo { get; set; }

    public DateTime? MachineStartTime { get; set; }

    public DateTime? MachineEndTime { get; set; }

    public int? MachineStatus { get; set; }

    public int? MachineIdleTime { get; set; }

    public int? MachineSetupTime { get; set; }

    public int? MachineBreakTime { get; set; }

    public int? MachineSetupDelayTime { get; set; }

    public int? MachineBreakDelayTime { get; set; }

    public int? MachineDownTime { get; set; }

    public int? MachineRunningTime { get; set; }

    public decimal? StandardCycleTime { get; set; }

    public bool? IsCountActual { get; set; }

    public bool? IsOeeCal { get; set; }

    public int? MachinePlanQty { get; set; }

    public int? MachineActualQty { get; set; }

    public int? MachineOkQty { get; set; }

    public int? MachineNgQty { get; set; }

    public decimal? MachineEfficiencyTarget { get; set; }

    public decimal? MachineOeeAvailability { get; set; }

    public decimal? MachineOeePerformance { get; set; }

    public decimal? MachineOeeQuality { get; set; }

    public decimal? MachineOeeTotal { get; set; }

    public string? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public string? ServerProcessName { get; set; }

    public DateTime? StampTime { get; set; }
}
