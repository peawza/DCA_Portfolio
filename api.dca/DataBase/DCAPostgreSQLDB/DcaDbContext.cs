using System;
using System.Collections.Generic;
using DCAPostgreSQLDB.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace DCAPostgreSQLDB;

public partial class DcaDbContext : DbContext
{
    public DcaDbContext(DbContextOptions<DcaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetPriceHistory> AssetPriceHistories { get; set; }

    public virtual DbSet<CashMovement> CashMovements { get; set; }

    public virtual DbSet<DcaOrderDetail> DcaOrderDetails { get; set; }

    public virtual DbSet<DcaOrderHeader> DcaOrderHeaders { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.AssetId).HasName("asset_pkey");

            entity.ToTable("asset", "ref");

            entity.HasIndex(e => new { e.Symbol, e.Market }, "ux_asset_symbol_market").IsUnique();

            entity.Property(e => e.AssetId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("asset_id");
            entity.Property(e => e.AssetType)
                .HasMaxLength(20)
                .HasColumnName("asset_type");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasColumnName("currency");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Market)
                .HasMaxLength(20)
                .HasColumnName("market");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Symbol)
                .HasMaxLength(20)
                .HasColumnName("symbol");
        });

        modelBuilder.Entity<AssetPriceHistory>(entity =>
        {
            entity.HasKey(e => new { e.AssetId, e.PriceDate }).HasName("asset_price_history_pkey");

            entity.ToTable("asset_price_history", "ref");

            entity.Property(e => e.AssetId).HasColumnName("asset_id");
            entity.Property(e => e.PriceDate).HasColumnName("price_date");
            entity.Property(e => e.ClosePrice)
                .HasPrecision(18, 4)
                .HasColumnName("close_price");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetPriceHistories)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asset_price_history_asset_id_fkey");
        });

        modelBuilder.Entity<CashMovement>(entity =>
        {
            entity.HasKey(e => e.CashMovementId).HasName("cash_movement_pkey");

            entity.ToTable("cash_movement", "dca");

            entity.Property(e => e.CashMovementId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("cash_movement_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.MovementDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("movement_date");
            entity.Property(e => e.MovementType)
                .HasMaxLength(20)
                .HasColumnName("movement_type");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
        });

        modelBuilder.Entity<DcaOrderDetail>(entity =>
        {
            entity.HasKey(e => e.DcaOrderDetailId).HasName("dca_order_detail_pkey");

            entity.ToTable("dca_order_detail", "dca");

            entity.Property(e => e.DcaOrderDetailId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("dca_order_detail_id");
            entity.Property(e => e.AssetType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'STOCK'::character varying")
                .HasColumnName("asset_type");
            entity.Property(e => e.BrokerOrderId)
                .HasMaxLength(50)
                .HasColumnName("broker_order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CurrentMarketValue)
                .HasPrecision(18, 2)
                .HasColumnName("current_market_value");
            entity.Property(e => e.DcaOrderId).HasColumnName("dca_order_id");
            entity.Property(e => e.ExecutedAmount)
                .HasPrecision(18, 2)
                .HasColumnName("executed_amount");
            entity.Property(e => e.ExecutedAt).HasColumnName("executed_at");
            entity.Property(e => e.ExecutedFlag)
                .HasDefaultValue(false)
                .HasColumnName("executed_flag");
            entity.Property(e => e.ExecutedPrice)
                .HasPrecision(18, 4)
                .HasColumnName("executed_price");
            entity.Property(e => e.Market)
                .HasMaxLength(20)
                .HasColumnName("market");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.OrderSide)
                .HasMaxLength(4)
                .HasDefaultValueSql("'BUY'::character varying")
                .HasColumnName("order_side");
            entity.Property(e => e.PlannedAmount)
                .HasPrecision(18, 2)
                .HasColumnName("planned_amount");
            entity.Property(e => e.PlannedPrice)
                .HasPrecision(18, 4)
                .HasColumnName("planned_price");
            entity.Property(e => e.RowNo).HasColumnName("row_no");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'PLANNED'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Symbol)
                .HasMaxLength(20)
                .HasColumnName("symbol");
            entity.Property(e => e.UnrealizedPnlValue)
                .HasPrecision(18, 2)
                .HasColumnName("unrealized_pnl_value");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");

            entity.HasOne(d => d.DcaOrder).WithMany(p => p.DcaOrderDetails)
                .HasForeignKey(d => d.DcaOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dca_order_detail_header");
        });

        modelBuilder.Entity<DcaOrderHeader>(entity =>
        {
            entity.HasKey(e => e.DcaOrderId).HasName("dca_order_header_pkey");

            entity.ToTable("dca_order_header", "dca");

            entity.Property(e => e.DcaOrderId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("dca_order_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.PlanName)
                .HasMaxLength(100)
                .HasColumnName("plan_name");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'PLANNED'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TargetBudget)
                .HasPrecision(18, 2)
                .HasColumnName("target_budget");
            entity.Property(e => e.TotalCurrentValue)
                .HasPrecision(18, 2)
                .HasColumnName("total_current_value");
            entity.Property(e => e.TotalExecutedValue)
                .HasPrecision(18, 2)
                .HasColumnName("total_executed_value");
            entity.Property(e => e.TotalFee)
                .HasPrecision(18, 2)
                .HasColumnName("total_fee");
            entity.Property(e => e.TotalPlannedValue)
                .HasPrecision(18, 2)
                .HasColumnName("total_planned_value");
            entity.Property(e => e.TotalUnrealizedPnl)
                .HasPrecision(18, 2)
                .HasColumnName("total_unrealized_pnl");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("position_pkey");

            entity.ToTable("position", "dca");

            entity.Property(e => e.PositionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("position_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.AssetId).HasColumnName("asset_id");
            entity.Property(e => e.AvgCost)
                .HasPrecision(18, 4)
                .HasColumnName("avg_cost");
            entity.Property(e => e.CurrentPrice)
                .HasPrecision(18, 4)
                .HasColumnName("current_price");
            entity.Property(e => e.CurrentValue)
                .HasPrecision(18, 2)
                .HasColumnName("current_value");
            entity.Property(e => e.RealizedPnl)
                .HasPrecision(18, 2)
                .HasColumnName("realized_pnl");
            entity.Property(e => e.TotalCost)
                .HasPrecision(18, 2)
                .HasColumnName("total_cost");
            entity.Property(e => e.TotalQty)
                .HasPrecision(18, 6)
                .HasColumnName("total_qty");
            entity.Property(e => e.UnrealizedPnl)
                .HasPrecision(18, 2)
                .HasColumnName("unrealized_pnl");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Asset).WithMany(p => p.Positions)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("position_asset_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
