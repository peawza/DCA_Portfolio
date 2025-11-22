using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TempMachinePostgreSQLDB.Models.TempMachine;

namespace TempMachinePostgreSQLDB;

public partial class TempMachineDbContext : DbContext
{
    public TempMachineDbContext(DbContextOptions<TempMachineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TempProcessHistoryMachineDetail> TempProcessHistoryMachineDetails { get; set; }

    public virtual DbSet<TempProcessMachine> TempProcessMachines { get; set; }

    public virtual DbSet<TempProcessMachineDetail> TempProcessMachineDetails { get; set; }

    public virtual DbSet<TempProcessMachineSummary> TempProcessMachineSummaries { get; set; }

    public virtual DbSet<TempProcessWipSummary> TempProcessWipSummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TempProcessHistoryMachineDetail>(entity =>
        {
            entity.HasKey(e => new { e.MachineCode, e.IndexTime }).HasName("temp_process_history_machine_details_pkey");

            entity
                .ToTable("temp_process_history_machine_details", "temp")
                .HasAnnotation("Npgsql:StorageParameter:fillfactor", "70");

            entity.Property(e => e.MachineCode)
                .HasMaxLength(50)
                .HasColumnName("machine_code");
            entity.Property(e => e.IndexTime).HasColumnName("index_time");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .HasColumnName("employee_name");
            entity.Property(e => e.MachineActualQty).HasColumnName("machine_actual_qty");
            entity.Property(e => e.MachineEfficiencyTarget)
                .HasPrecision(5, 2)
                .HasColumnName("machine_efficiency_target");
            entity.Property(e => e.MachineNgQty).HasColumnName("machine_ng_qty");
            entity.Property(e => e.MachineOeeAvailability)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_availability");
            entity.Property(e => e.MachineOeePerformance)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_performance");
            entity.Property(e => e.MachineOeeQuality)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_quality");
            entity.Property(e => e.MachineOeeTotal)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_total");
            entity.Property(e => e.MachineOkQty).HasColumnName("machine_ok_qty");
            entity.Property(e => e.MachinePlanQty).HasColumnName("machine_plan_qty");
            entity.Property(e => e.MachineStatus).HasColumnName("machine_status");
            entity.Property(e => e.ProcessDate).HasColumnName("process_date");
            entity.Property(e => e.ServerProcessName)
                .HasMaxLength(50)
                .HasColumnName("server_process_name");
            entity.Property(e => e.ShiftId)
                .HasMaxLength(50)
                .HasColumnName("shift_id");
            entity.Property(e => e.StampTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stamp_time");
            entity.Property(e => e.StandardCycleTime)
                .HasPrecision(10, 2)
                .HasColumnName("standard_cycle_time");
            entity.Property(e => e.TimeSeries).HasColumnName("time_series");
            entity.Property(e => e.WipActualQty).HasColumnName("wip_actual_qty");
            entity.Property(e => e.WipEfficiencyTarget)
                .HasPrecision(5, 2)
                .HasColumnName("wip_efficiency_target");
            entity.Property(e => e.WipLabelId).HasColumnName("wip_label_id");
            entity.Property(e => e.WipLabelNo)
                .HasMaxLength(50)
                .HasColumnName("wip_label_no");
            entity.Property(e => e.WipNgQty).HasColumnName("wip_ng_qty");
            entity.Property(e => e.WipOeeAvailability)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_availability");
            entity.Property(e => e.WipOeePerformance)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_performance");
            entity.Property(e => e.WipOeeQuality)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_quality");
            entity.Property(e => e.WipOeeTotal)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_total");
            entity.Property(e => e.WipOkQty).HasColumnName("wip_ok_qty");
            entity.Property(e => e.WipPlanQty).HasColumnName("wip_plan_qty");
            entity.Property(e => e.WipStatus).HasColumnName("wip_status");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");
            entity.Property(e => e.WorkOrderNo)
                .HasMaxLength(50)
                .HasColumnName("work_order_no");
        });

        modelBuilder.Entity<TempProcessMachine>(entity =>
        {
            entity.HasKey(e => new { e.MachineCode, e.ShiftId }).HasName("temp_process_machine_pkey");

            entity.ToTable("temp_process_machine", "temp");

            entity.Property(e => e.MachineCode)
                .HasMaxLength(50)
                .HasColumnName("machine_code");
            entity.Property(e => e.ShiftId)
                .HasMaxLength(50)
                .HasColumnName("shift_id");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .HasColumnName("employee_name");
            entity.Property(e => e.MachineActualQty).HasColumnName("machine_actual_qty");
            entity.Property(e => e.MachineEndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("machine_end_time");
            entity.Property(e => e.MachineNgQty).HasColumnName("machine_ng_qty");
            entity.Property(e => e.MachineOkQty).HasColumnName("machine_ok_qty");
            entity.Property(e => e.MachinePlanQty).HasColumnName("machine_plan_qty");
            entity.Property(e => e.MachineShiftStatus).HasColumnName("machine_shift_status");
            entity.Property(e => e.MachineStartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("machine_start_time");
            entity.Property(e => e.MachineStatus).HasColumnName("machine_status");
            entity.Property(e => e.ServerProcessName)
                .HasMaxLength(50)
                .HasColumnName("server_process_name");
            entity.Property(e => e.StampTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stamp_time");
            entity.Property(e => e.StandardCycleTime)
                .HasPrecision(10, 2)
                .HasColumnName("standard_cycle_time");
            entity.Property(e => e.WipActualQty).HasColumnName("wip_actual_qty");
            entity.Property(e => e.WipLabelId).HasColumnName("wip_label_id");
            entity.Property(e => e.WipLabelNo)
                .HasMaxLength(50)
                .HasColumnName("wip_label_no");
            entity.Property(e => e.WipNgQty).HasColumnName("wip_ng_qty");
            entity.Property(e => e.WipOkQty).HasColumnName("wip_ok_qty");
            entity.Property(e => e.WipPlanQty).HasColumnName("wip_plan_qty");
            entity.Property(e => e.WipStatus).HasColumnName("wip_status");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");
            entity.Property(e => e.WorkOrderNo)
                .HasMaxLength(50)
                .HasColumnName("work_order_no");
        });

        modelBuilder.Entity<TempProcessMachineDetail>(entity =>
        {
            entity.HasKey(e => new { e.MachineCode, e.IndexTime }).HasName("temp_process_machine_details_pkey");

            entity
                .ToTable("temp_process_machine_details", "temp")
                .HasAnnotation("Npgsql:StorageParameter:fillfactor", "70");

            entity.HasIndex(e => new { e.MachineCode, e.IndexTime }, "idx_temp_machine_code_index_time");

            entity.HasIndex(e => e.MachineStatus, "idx_temp_machine_status");

            entity.HasIndex(e => e.ShiftId, "idx_temp_shift_id");

            entity.Property(e => e.MachineCode)
                .HasMaxLength(50)
                .HasColumnName("machine_code");
            entity.Property(e => e.IndexTime).HasColumnName("index_time");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .HasColumnName("employee_name");
            entity.Property(e => e.MachineActualQty).HasColumnName("machine_actual_qty");
            entity.Property(e => e.MachineEfficiencyTarget)
                .HasPrecision(5, 2)
                .HasColumnName("machine_efficiency_target");
            entity.Property(e => e.MachineNgQty).HasColumnName("machine_ng_qty");
            entity.Property(e => e.MachineOeeAvailability)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_availability");
            entity.Property(e => e.MachineOeePerformance)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_performance");
            entity.Property(e => e.MachineOeeQuality)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_quality");
            entity.Property(e => e.MachineOeeTotal)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_total");
            entity.Property(e => e.MachineOkQty).HasColumnName("machine_ok_qty");
            entity.Property(e => e.MachinePlanQty).HasColumnName("machine_plan_qty");
            entity.Property(e => e.MachineStatus).HasColumnName("machine_status");
            entity.Property(e => e.ServerProcessName)
                .HasMaxLength(50)
                .HasColumnName("server_process_name");
            entity.Property(e => e.ShiftId)
                .HasMaxLength(50)
                .HasColumnName("shift_id");
            entity.Property(e => e.StampTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stamp_time");
            entity.Property(e => e.StandardCycleTime)
                .HasPrecision(10, 2)
                .HasColumnName("standard_cycle_time");
            entity.Property(e => e.TimeSeries).HasColumnName("time_series");
            entity.Property(e => e.WipActualQty).HasColumnName("wip_actual_qty");
            entity.Property(e => e.WipEfficiencyTarget)
                .HasPrecision(5, 2)
                .HasColumnName("wip_efficiency_target");
            entity.Property(e => e.WipLabelId).HasColumnName("wip_label_id");
            entity.Property(e => e.WipLabelNo)
                .HasMaxLength(50)
                .HasColumnName("wip_label_no");
            entity.Property(e => e.WipNgQty).HasColumnName("wip_ng_qty");
            entity.Property(e => e.WipOeeAvailability)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_availability");
            entity.Property(e => e.WipOeePerformance)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_performance");
            entity.Property(e => e.WipOeeQuality)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_quality");
            entity.Property(e => e.WipOeeTotal)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_total");
            entity.Property(e => e.WipOkQty).HasColumnName("wip_ok_qty");
            entity.Property(e => e.WipPlanQty).HasColumnName("wip_plan_qty");
            entity.Property(e => e.WipStatus).HasColumnName("wip_status");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");
            entity.Property(e => e.WorkOrderNo)
                .HasMaxLength(50)
                .HasColumnName("work_order_no");
        });

        modelBuilder.Entity<TempProcessMachineSummary>(entity =>
        {
            entity.HasKey(e => new { e.MachineCode, e.ShiftId, e.WipLabelId, e.WorkOrderId }).HasName("temp_process_machines_summary");

            entity.ToTable("temp_process_machine_summary", "temp");

            entity.Property(e => e.MachineCode)
                .HasMaxLength(50)
                .HasColumnName("machine_code");
            entity.Property(e => e.ShiftId)
                .HasMaxLength(50)
                .HasColumnName("shift_id");
            entity.Property(e => e.WipLabelId).HasColumnName("wip_label_id");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("employee_id");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .HasColumnName("employee_name");
            entity.Property(e => e.IsCountActual).HasColumnName("is_count_actual");
            entity.Property(e => e.IsOeeCal).HasColumnName("is_oee_cal");
            entity.Property(e => e.MachineActualQty).HasColumnName("machine_actual_qty");
            entity.Property(e => e.MachineBreakDelayTime).HasColumnName("machine_break_delay_time");
            entity.Property(e => e.MachineBreakTime).HasColumnName("machine_break_time");
            entity.Property(e => e.MachineDownTime).HasColumnName("machine_down_time");
            entity.Property(e => e.MachineEfficiencyTarget)
                .HasPrecision(5, 2)
                .HasColumnName("machine_efficiency_target");
            entity.Property(e => e.MachineEndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("machine_end_time");
            entity.Property(e => e.MachineIdleTime).HasColumnName("machine_idle_time");
            entity.Property(e => e.MachineNgQty).HasColumnName("machine_ng_qty");
            entity.Property(e => e.MachineOeeAvailability)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_availability");
            entity.Property(e => e.MachineOeePerformance)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_performance");
            entity.Property(e => e.MachineOeeQuality)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_quality");
            entity.Property(e => e.MachineOeeTotal)
                .HasPrecision(5, 2)
                .HasColumnName("machine_oee_total");
            entity.Property(e => e.MachineOkQty).HasColumnName("machine_ok_qty");
            entity.Property(e => e.MachinePlanQty).HasColumnName("machine_plan_qty");
            entity.Property(e => e.MachineRunningTime).HasColumnName("machine_running_time");
            entity.Property(e => e.MachineSetupDelayTime).HasColumnName("machine_setup_delay_time");
            entity.Property(e => e.MachineSetupTime).HasColumnName("machine_setup_time");
            entity.Property(e => e.MachineStartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("machine_start_time");
            entity.Property(e => e.MachineStatus).HasColumnName("machine_status");
            entity.Property(e => e.ServerProcessName)
                .HasMaxLength(50)
                .HasColumnName("server_process_name");
            entity.Property(e => e.StampTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stamp_time");
            entity.Property(e => e.StandardCycleTime)
                .HasPrecision(10, 2)
                .HasColumnName("standard_cycle_time");
            entity.Property(e => e.WipLabelNo)
                .HasMaxLength(50)
                .HasColumnName("wip_label_no");
            entity.Property(e => e.WorkOrderNo)
                .HasMaxLength(50)
                .HasColumnName("work_order_no");
        });

        modelBuilder.Entity<TempProcessWipSummary>(entity =>
        {
            entity.HasKey(e => new { e.MachineCode, e.ShiftId, e.WipLabelId, e.WorkOrderId }).HasName("temp_process_wip_summary_pkey");

            entity.ToTable("temp_process_wip_summary", "temp");

            entity.Property(e => e.MachineCode)
                .HasMaxLength(50)
                .HasColumnName("machine_code");
            entity.Property(e => e.ShiftId)
                .HasMaxLength(50)
                .HasColumnName("shift_id");
            entity.Property(e => e.WipLabelId).HasColumnName("wip_label_id");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("employee_id");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .HasColumnName("employee_name");
            entity.Property(e => e.IsCountActual).HasColumnName("is_count_actual");
            entity.Property(e => e.PlanQty).HasColumnName("plan_qty");
            entity.Property(e => e.ServerProcessName)
                .HasMaxLength(50)
                .HasColumnName("server_process_name");
            entity.Property(e => e.StampTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stamp_time");
            entity.Property(e => e.StandardCycleTime)
                .HasPrecision(10, 2)
                .HasColumnName("standard_cycle_time");
            entity.Property(e => e.WipActualQty).HasColumnName("wip_actual_qty");
            entity.Property(e => e.WipBreakDelayTime).HasColumnName("wip_break_delay_time");
            entity.Property(e => e.WipBreakTime).HasColumnName("wip_break_time");
            entity.Property(e => e.WipDownTime).HasColumnName("wip_down_time");
            entity.Property(e => e.WipEfficiencyTarget)
                .HasPrecision(5, 2)
                .HasColumnName("wip_efficiency_target");
            entity.Property(e => e.WipEndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("wip_end_time");
            entity.Property(e => e.WipIdleTime).HasColumnName("wip_idle_time");
            entity.Property(e => e.WipLabelNo)
                .HasMaxLength(50)
                .HasColumnName("wip_label_no");
            entity.Property(e => e.WipNgQty).HasColumnName("wip_ng_qty");
            entity.Property(e => e.WipOeeAvailability)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_availability");
            entity.Property(e => e.WipOeePerformance)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_performance");
            entity.Property(e => e.WipOeeQuality)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_quality");
            entity.Property(e => e.WipOeeTotal)
                .HasPrecision(5, 2)
                .HasColumnName("wip_oee_total");
            entity.Property(e => e.WipOkQty).HasColumnName("wip_ok_qty");
            entity.Property(e => e.WipRunningTime).HasColumnName("wip_running_time");
            entity.Property(e => e.WipSetupDelayTime).HasColumnName("wip_setup_delay_time");
            entity.Property(e => e.WipSetupTime).HasColumnName("wip_setup_time");
            entity.Property(e => e.WipStartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("wip_start_time");
            entity.Property(e => e.WipStatus).HasColumnName("wip_status");
            entity.Property(e => e.WorkOrderNo)
                .HasMaxLength(50)
                .HasColumnName("work_order_no");
            entity.Property(e => e.WorkOrderType)
                .HasMaxLength(100)
                .HasColumnName("work_order_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
