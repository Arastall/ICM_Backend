using System;
using System.Collections.Generic;
using ICMServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ICMServer.DBContext;

public partial class ICMDBContext : DbContext
{
    public ICMDBContext()
    {
    }

    public ICMDBContext(DbContextOptions<ICMDBContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<BulkAdjustmentLoad> BulkAdjustmentLoads { get; set; }

    public virtual DbSet<ConfigAllocationType> ConfigAllocationTypes { get; set; }

    //public virtual DbSet<ConfigBonu> ConfigBonus { get; set; }

    //public virtual DbSet<ConfigCommissionMultiplier> ConfigCommissionMultipliers { get; set; }

    //public virtual DbSet<ConfigCommissionMultiplierAllocation> ConfigCommissionMultiplierAllocations { get; set; }

    //public virtual DbSet<ConfigCommissionProduct> ConfigCommissionProducts { get; set; }

    //public virtual DbSet<ConfigExclusionRep> ConfigExclusionReps { get; set; }

    //public virtual DbSet<ConfigFpsm> ConfigFpsms { get; set; }

    public virtual DbSet<ConfigIcmAdminGroup> ConfigIcmAdminGroups { get; set; }

    public virtual DbSet<ConfigIcmAdminScreen> ConfigIcmAdminScreens { get; set; }

    public virtual DbSet<ConfigIcmAdminUser> ConfigIcmAdminUsers { get; set; }

    //public virtual DbSet<ConfigIncludedServiceProduct> ConfigIncludedServiceProducts { get; set; }

    //public virtual DbSet<ConfigMonthlyBonu> ConfigMonthlyBonus { get; set; }

    public virtual DbSet<ConfigOrderStatus> ConfigOrderStatuses { get; set; }

    //public virtual DbSet<ConfigPayplkanAllocationLink> ConfigPayplanAllocationLinks { get; set; }

    //public virtual DbSet<ConfigPayplanEngDeductionRate> ConfigPayplanEngDeductionRates { get; set; }

    public virtual DbSet<ConfigPayplanEnhancement> ConfigPayplanEnhancements { get; set; }

    public virtual DbSet<ConfigPayplanRateTable> ConfigPayplanRateTables { get; set; }

    public virtual DbSet<ConfigPayplanType> ConfigPayplanTypes { get; set; }

    //public virtual DbSet<ConfigPerformanceThreshold> ConfigPerformanceThresholds { get; set; }

    //public virtual DbSet<ConfigPerformanceThreshold2016> ConfigPerformanceThreshold2016s { get; set; }

    //public virtual DbSet<ConfigPerformanceThresholdPeriodOverride> ConfigPerformanceThresholdPeriodOverrides { get; set; }

    public virtual DbSet<ConfigProcessingType> ConfigProcessingTypes { get; set; }

    //public virtual DbSet<ConfigPromoUplift> ConfigPromoUplifts { get; set; }

    //public virtual DbSet<ConfigRateTableCriterion> ConfigRateTableCriteria { get; set; }

    //public virtual DbSet<ConfigRateTableValue> ConfigRateTableValues { get; set; }

    public virtual DbSet<ConfigRollupType> ConfigRollupTypes { get; set; }

    public virtual DbSet<ConfigSalesPeriod> ConfigSalesPeriods { get; set; }

    public virtual DbSet<ConfigSystemParameter> ConfigSystemParameters { get; set; }

    public virtual DbSet<DataBusinessUnit> DataBusinessUnits { get; set; }

    public virtual DbSet<DataCancelReplaceSkippedOrder> DataCancelReplaceSkippedOrders { get; set; }

    //public virtual DbSet<DataCommisionPaymentExcludeFromPerformanceDeduction> DataCommisionPaymentExcludeFromPerformanceDeductions { get; set; }

    //public virtual DbSet<DataCommisionPaymentThresholdReleased> DataCommisionPaymentThresholdReleaseds { get; set; }

    public virtual DbSet<DataCommissionPayment> DataCommissionPayments { get; set; }

    //public virtual DbSet<DataCommissionPaymentOverride> DataCommissionPaymentOverrides { get; set; }

    //public virtual DbSet<DataCommissionRateMatrix> DataCommissionRateMatrices { get; set; }

    //public virtual DbSet<DataCommissionsMultiplier> DataCommissionsMultipliers { get; set; }

    public virtual DbSet<DataCreditAllocation> DataCreditAllocations { get; set; }

    //public virtual DbSet<DataCutoffOrder> DataCutoffOrders { get; set; }

    public virtual DbSet<DataDefaultTarget> DataDefaultTargets { get; set; }

    public virtual DbSet<DataEmployee> DataEmployees { get; set; }

    public virtual DbSet<DataEmployeeTarget> DataEmployeeTargets { get; set; }

    public virtual DbSet<DataExcludedProduct> DataExcludedProducts { get; set; }

    //public virtual DbSet<DataFollowUpEntitlement> DataFollowUpEntitlements { get; set; }

    //public virtual DbSet<DataFollowUpPayment> DataFollowUpPayments { get; set; }

    //public virtual DbSet<DataImportOverride> DataImportOverrides { get; set; }

    public virtual DbSet<DataImpressCommissionIncident> DataImpressCommissionIncidents { get; set; }

    public virtual DbSet<DataImpressSubscriptionOrder> DataImpressSubscriptionOrders { get; set; }

    public virtual DbSet<DataManualAdjustment> DataManualAdjustments { get; set; }

    //public virtual DbSet<DataMatrixCommissionRate> DataMatrixCommissionRates { get; set; }

    //public virtual DbSet<DataNegativeCommission> DataNegativeCommissions { get; set; }

    public virtual DbSet<DataNewcomerSetting> DataNewcomerSettings { get; set; }

    public virtual DbSet<DataOrderHeader> DataOrderHeaders { get; set; }

    public virtual DbSet<DataOrderItem> DataOrderItems { get; set; }

    public virtual DbSet<DataOrderPosition> DataOrderPositions { get; set; }

    public virtual DbSet<DataOrderProcessHistory> DataOrderProcessHistories { get; set; }

    public virtual DbSet<DataOrderServiceItem> DataOrderServiceItems { get; set; }

    public virtual DbSet<DataOrderStatusHistory> DataOrderStatusHistories { get; set; }

    //public virtual DbSet<DataPayPlanTypeChange> DataPayPlanTypeChanges { get; set; }

    public virtual DbSet<DataPerformanceAgainstTargetMatrix> DataPerformanceAgainstTargetMatrices { get; set; }

    //public virtual DbSet<DataPerformanceThreshold> DataPerformanceThresholds { get; set; }

    public virtual DbSet<DataPositionHistory> DataPositionHistories { get; set; }

    public virtual DbSet<DataProductsIncluded> DataProductsIncludeds { get; set; }

    public virtual DbSet<DataProductsUnknown> DataProductsUnknowns { get; set; }

    //public virtual DbSet<DataPsoRate> DataPsoRates { get; set; }

    //public virtual DbSet<DataRevenueAgainstOrder> DataRevenueAgainstOrders { get; set; }

    //public virtual DbSet<DataRevenueTargetBucket> DataRevenueTargetBuckets { get; set; }

    //public virtual DbSet<DataRevenueUplift> DataRevenueUplifts { get; set; }

    //public virtual DbSet<DataUnitsOverride> DataUnitsOverrides { get; set; }

    //public virtual DbSet<DeductionExclusionRep> DeductionExclusionReps { get; set; }

    //public virtual DbSet<FpsmAllocation> FpsmAllocations { get; set; }

    //public virtual DbSet<FpsmTempAllocation> FpsmTempAllocations { get; set; }

    public virtual DbSet<OrdersToProcess> OrdersToProcesses { get; set; }

    public virtual DbSet<PayplanUnset> PayplanUnsets { get; set; }

    public virtual DbSet<RunIcmInfo> RunIcmInfos { get; set; }

    //public virtual DbSet<SupplyAdjustment> SupplyAdjustments { get; set; }

    public virtual DbSet<SystemLog> SystemLogs { get; set; }

    public virtual DbSet<TmpDataEmployee> TmpDataEmployees { get; set; }

    public virtual DbSet<TmpDataImpressSubscriptionOrder> TmpDataImpressSubscriptionOrders { get; set; }

    public virtual DbSet<TmpDataOrderHeader> TmpDataOrderHeaders { get; set; }

    public virtual DbSet<TmpDataOrderItem> TmpDataOrderItems { get; set; }

    public virtual DbSet<TmpDataOrderRep> TmpDataOrderReps { get; set; }

    public virtual DbSet<TmpDataOrderStatusHistory> TmpDataOrderStatusHistories { get; set; }

    public virtual DbSet<TmpDataPosition> TmpDataPositions { get; set; }

    //public virtual DbSet<VTblTargetReport> VTblTargetReports { get; set; }

    //public virtual DbSet<VTblTargetReportS6> VTblTargetReportS6s { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ne configurez que si pas déjà configuré ET pas en pooling
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("iis"));
        }
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_BIN");

        //modelBuilder.Entity<BulkAdjustmentLoad>(entity =>
        //{
        //    entity.HasKey(e => e.RowId)
        //        .HasName("PK__BULK_ADJUSTMENT___6B28E335")
        //        .HasFillFactor(90);

        //    entity.ToTable("BULK_ADJUSTMENT_LOAD");

        //    entity.Property(e => e.RowId).HasColumnName("rowId");
        //    entity.Property(e => e.BmName)
        //        .HasMaxLength(255)
        //        .HasColumnName("bmName");
        //    entity.Property(e => e.Commission)
        //        .HasColumnType("numeric(21, 7)")
        //        .HasColumnName("commission");
        //    entity.Property(e => e.Description)
        //        .HasMaxLength(255)
        //        .HasColumnName("description");
        //    entity.Property(e => e.Position)
        //        .HasMaxLength(50)
        //        .HasColumnName("position");
        //    entity.Property(e => e.Revenue)
        //        .HasColumnType("numeric(21, 7)")
        //        .HasColumnName("revenue");
        //    entity.Property(e => e.Type)
        //        .HasMaxLength(50)
        //        .HasColumnName("type");
        //});

        modelBuilder.Entity<ConfigAllocationType>(entity =>
        {
            entity.HasKey(e => e.AllocationTypeId)
                .HasName("PK_ALLOCATION_TYPE_ID")
                .HasFillFactor(90);

            entity.ToTable("CONFIG_ALLOCATION_TYPE");

            entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
            entity.Property(e => e.AllocationCriteriaSql).HasColumnName("ALLOCATION_CRITERIA_SQL");
            entity.Property(e => e.AllocationDescription)
                .HasMaxLength(500)
                .HasColumnName("ALLOCATION_DESCRIPTION");
            entity.Property(e => e.AllocationType)
                .HasMaxLength(50)
                .HasColumnName("ALLOCATION_TYPE");
        });

        //modelBuilder.Entity<ConfigBonu>(entity =>
        //{
        //    entity.HasKey(e => e.ConfigBonusId).HasFillFactor(90);

        //    entity.ToTable("CONFIG_BONUS");

        //    entity.Property(e => e.ConfigBonusId).HasColumnName("CONFIG_BONUS_ID");
        //    entity.Property(e => e.AllocationTypeId)
        //        .HasComment("This will be the ID as on CONFIG_ALLOCATION_TYPE")
        //        .HasColumnName("ALLOCATION_TYPE_ID");
        //    entity.Property(e => e.Bonus)
        //        .HasComment("Actual bonus value to be paid")
        //        .HasColumnType("numeric(9, 2)")
        //        .HasColumnName("BONUS");
        //    entity.Property(e => e.BonusType)
        //        .HasMaxLength(7)
        //        .HasDefaultValue("QUARTER")
        //        .HasComment("BONUS_TYPE - either QUARTER, ANNUAL or RETRO")
        //        .HasColumnName("BONUS_TYPE");
        //    entity.Property(e => e.Description)
        //        .HasMaxLength(500)
        //        .HasColumnName("DESCRIPTION");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasDefaultValue("X")
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.FinYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("FIN_YEAR");
        //    entity.Property(e => e.IsChild)
        //        .HasMaxLength(1)
        //        .HasDefaultValue("N")
        //        .HasComment("Indicates if the bonus line is a child value used to determine whether or not PARENT bonus can be released")
        //        .HasColumnName("IS_CHILD");
        //    entity.Property(e => e.ParentBonusId)
        //        .HasComment("if the IS_CHILD field is set to true then this must point to the parent bonus held in this table (iterative link)")
        //        .HasColumnName("PARENT_BONUS_ID");
        //    entity.Property(e => e.PayPlanType)
        //        .HasMaxLength(5)
        //        .HasDefaultValue("XXXXX")
        //        .HasColumnName("PAY_PLAN_TYPE");
        //    entity.Property(e => e.Percentage)
        //        .HasDefaultValue(100.00m)
        //        .HasColumnType("numeric(9, 2)")
        //        .HasColumnName("PERCENTAGE");
        //    entity.Property(e => e.RangeFrom)
        //        .HasColumnType("numeric(9, 2)")
        //        .HasColumnName("RANGE_FROM");
        //    entity.Property(e => e.RangeTo)
        //        .HasColumnType("numeric(9, 2)")
        //        .HasColumnName("RANGE_TO");
        //});

        //modelBuilder.Entity<ConfigCommissionMultiplier>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasFillFactor(90);

        //    entity.ToTable("CONFIG_COMMISSION_MULTIPLIER");

        //    entity.HasIndex(e => new { e.PayPlan, e.EmployeeId }, "UK_CONFIG_COMMISSION_MULTIPLIER_PayPlanEmployeeID")
        //        .IsUnique()
        //        .HasFillFactor(90);

        //    entity.Property(e => e.Id).HasColumnName("ID");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(60)
        //        .HasColumnName("EmployeeID");
        //    entity.Property(e => e.EnhancementRate).HasColumnType("numeric(5, 2)");
        //    entity.Property(e => e.PayPlan).HasMaxLength(30);
        //});

        //modelBuilder.Entity<ConfigCommissionMultiplierAllocation>(entity =>
        //{
        //    entity.HasKey(e => new { e.ConfigCommissionMultiplierId, e.AllocationTypeId }).HasFillFactor(90);

        //    entity.ToTable("CONFIG_COMMISSION_MULTIPLIER_ALLOCATIONS");

        //    entity.Property(e => e.ConfigCommissionMultiplierId).HasColumnName("CONFIG_COMMISSION_MULTIPLIER_ID");
        //    entity.Property(e => e.AllocationTypeId).HasColumnName("AllocationTypeID");
        //});

        //modelBuilder.Entity<ConfigCommissionProduct>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_COMMISSION_PRODUCTS");

        //    entity.Property(e => e.BuName)
        //        .HasMaxLength(50)
        //        .IsUnicode(false)
        //        .HasColumnName("BU_NAME");
        //    entity.Property(e => e.Process)
        //        .HasDefaultValue(false)
        //        .HasColumnName("PROCESS");
        //    entity.Property(e => e.ProductLevel1)
        //        .HasMaxLength(30)
        //        .HasColumnName("PRODUCT_LEVEL_1");
        //    entity.Property(e => e.ProductLevel1Description)
        //        .HasMaxLength(200)
        //        .IsUnicode(false)
        //        .HasColumnName("PRODUCT_LEVEL_1_DESCRIPTION");
        //});

        //modelBuilder.Entity<ConfigExclusionRep>(entity =>
        //{
        //    entity.HasKey(e => e.ConfigExclusionReps).HasFillFactor(90);

        //    entity.ToTable("CONFIG_EXCLUSION_REPS");

        //    entity.Property(e => e.ConfigExclusionReps).HasColumnName("CONFIG_EXCLUSION_REPS");
        //    entity.Property(e => e.ConfigBonusId).HasColumnName("CONFIG_BONUS_ID");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
        //        .HasColumnName("PERIOD_MONTH");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("PERIOD_YEAR");
        //    entity.Property(e => e.Reason).HasColumnName("REASON");
        //    entity.Property(e => e.RepName).HasColumnName("REP_NAME");
        //    entity.Property(e => e.Status).HasColumnName("STATUS");
        //});

        //modelBuilder.Entity<ConfigFpsm>(entity =>
        //{
        //    entity.HasKey(e => e.ConfigFpsmId)
        //        .HasName("PK_CONFIG_FPSM_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("CONFIG_FPSM");

        //    entity.HasIndex(e => e.EmployeeId, "IX_CONFIG_FPSM_EMPLOYEE_ID").HasFillFactor(90);

        //    entity.HasIndex(e => e.PositionId, "IX_CONFIG_FPSM_POSITION_ID").HasFillFactor(90);

        //    entity.Property(e => e.ConfigFpsmId).HasColumnName("CONFIG_FPSM_ID");
        //    entity.Property(e => e.DestId).HasColumnName("DEST_ID");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.FinYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("FIN_YEAR");
        //    entity.Property(e => e.PositionId)
        //        .HasMaxLength(30)
        //        .HasColumnName("POSITION_ID");
        //    entity.Property(e => e.Region)
        //        .HasMaxLength(30)
        //        .HasColumnName("REGION");
        //    entity.Property(e => e.RgnStr)
        //        .HasMaxLength(20)
        //        .HasColumnName("RGN_STR");
        //    entity.Property(e => e.SqlExtension)
        //        .HasDefaultValue("n/a")
        //        .HasColumnName("SQL_EXTENSION");
        //    entity.Property(e => e.SrcId).HasColumnName("SRC_ID");
        //});

        modelBuilder.Entity<ConfigIcmAdminGroup>(entity =>
        {
            entity.HasKey(e => e.IcmGroupId)
                .HasName("PK_ICM_GROUP_ID")
                .HasFillFactor(90);

            entity.ToTable("CONFIG_ICM_ADMIN_GROUPS");

            entity.Property(e => e.IcmGroupId).HasColumnName("ICM_GROUP_ID");
            entity.Property(e => e.GroupDesc)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("GROUP_DESC");
        });

        modelBuilder.Entity<ConfigIcmAdminScreen>(entity =>
        {
            entity.HasKey(e => e.IcmScreenId)
                .HasName("PK_ICM_SCREEN_ID")
                .HasFillFactor(90);

            entity.ToTable("CONFIG_ICM_ADMIN_SCREENS");

            entity.Property(e => e.IcmScreenId).HasColumnName("ICM_SCREEN_ID");
            entity.Property(e => e.GroupId).HasColumnName("GROUP_ID");
            entity.Property(e => e.ScreenName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SCREEN_NAME");
            entity.Property(e => e.ScreenTitle)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SCREEN_TITLE");
        });

        modelBuilder.Entity<ConfigIcmAdminUser>(entity =>
        {
            entity.HasKey(e => e.IcmUserId)
                .HasName("PK_ICM_USER_ID")
                .HasFillFactor(90);

            entity.ToTable("CONFIG_ICM_ADMIN_USERS");

            entity.Property(e => e.IcmUserId).HasColumnName("ICM_USER_ID");
            entity.Property(e => e.UserFname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_FNAME");
            entity.Property(e => e.UserGroupid).HasColumnName("USER_GROUPID");
            entity.Property(e => e.UserSname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_SNAME");
            entity.Property(e => e.Username)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        //modelBuilder.Entity<ConfigIncludedServiceProduct>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_INCLUDED_SERVICE_PRODUCTS");

        //    entity.Property(e => e.ProductCode)
        //        .HasMaxLength(200)
        //        .HasColumnName("productCode");
        //    entity.Property(e => e.ProductId)
        //        .HasMaxLength(50)
        //        .HasColumnName("productId");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //});

        //modelBuilder.Entity<ConfigMonthlyBonu>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_MONTHLY_BONUS");

        //    entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
        //    entity.Property(e => e.Bonus)
        //        .HasColumnType("numeric(19, 2)")
        //        .HasColumnName("BONUS");
        //    entity.Property(e => e.BonusType)
        //        .HasMaxLength(10)
        //        .HasDefaultValue("MONTHLY")
        //        .HasColumnName("BONUS_TYPE");
        //    entity.Property(e => e.ConfigMonthlyBonusId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("CONFIG_MONTHLY_BONUS_ID");
        //    entity.Property(e => e.Description)
        //        .HasMaxLength(100)
        //        .HasColumnName("DESCRIPTION");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(5)
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.FinYear)
        //        .HasMaxLength(5)
        //        .HasColumnName("FIN_YEAR");
        //    entity.Property(e => e.MonthlyTrigger)
        //        .HasColumnType("numeric(19, 2)")
        //        .HasColumnName("MONTHLY_TRIGGER");
        //    entity.Property(e => e.PayPlanType)
        //        .HasMaxLength(10)
        //        .HasColumnName("PAY_PLAN_TYPE");
        //});

        modelBuilder.Entity<ConfigOrderStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CONFIG_ORDER_STATUS");

            entity.Property(e => e.BuName)
                .HasMaxLength(60)
                .HasColumnName("BU_NAME");
            entity.Property(e => e.Commission)
                .HasDefaultValue(false)
                .HasColumnName("COMMISSION");
            entity.Property(e => e.Revenue)
                .HasDefaultValue(false)
                .HasColumnName("REVENUE");
            entity.Property(e => e.StatusCd)
                .HasMaxLength(200)
                .HasColumnName("STATUS_CD");
        });

        //modelBuilder.Entity<ConfigPayplanAllocationLink>(entity =>
        //{
        //    entity.HasKey(e => e.IcmPayplanAllId)
        //        .HasName("PK_ICM_PAYPLAN_ALL_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("CONFIG_PAYPLAN_ALLOCATION_LINK");

        //    entity.Property(e => e.IcmPayplanAllId).HasColumnName("ICM_PAYPLAN_ALL_ID");
        //    entity.Property(e => e.AllocationId).HasColumnName("ALLOCATION_ID");
        //    entity.Property(e => e.PayplanType)
        //        .HasMaxLength(5)
        //        .HasColumnName("PAYPLAN_TYPE");
        //});

        //modelBuilder.Entity<ConfigPayplanEngDeductionRate>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_PAYPLAN_ENG_DEDUCTION_RATE");

        //    entity.Property(e => e.DeductionRate)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("DEDUCTION_RATE");
        //    entity.Property(e => e.DeductionRateId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("DEDUCTION_RATE_ID");
        //    entity.Property(e => e.MaxDeductionValue)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("MAX_DEDUCTION_VALUE");
        //    entity.Property(e => e.PayplanType)
        //        .HasMaxLength(5)
        //        .IsUnicode(false)
        //        .HasColumnName("PAYPLAN_TYPE");
        //});

        modelBuilder.Entity<ConfigPayplanEnhancement>(entity =>
        {
            entity.HasKey(e => e.EnhancementId).HasFillFactor(90);

            entity.ToTable("CONFIG_PAYPLAN_ENHANCEMENT");

            entity.Property(e => e.EnhancementId).HasColumnName("ENHANCEMENT_ID");
            entity.Property(e => e.EnhancementCriteriaSql).HasColumnName("ENHANCEMENT_CRITERIA_SQL");
            entity.Property(e => e.EnhancementDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ENHANCEMENT_DESC");
            entity.Property(e => e.EnhancementRate)
                .HasColumnType("numeric(18, 7)")
                .HasColumnName("ENHANCEMENT_RATE");
            entity.Property(e => e.PayplanType)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("PAYPLAN_TYPE");
        });

        modelBuilder.Entity<ConfigPayplanRateTable>(entity =>
        {
            entity.HasKey(e => e.RateTableId)
                .HasName("PK_CONFIG_TABLE2")
                .HasFillFactor(90);

            entity.ToTable("CONFIG_PAYPLAN_RATE_TABLES");

            entity.Property(e => e.RateTableId).HasColumnName("RATE_TABLE_ID");
            entity.Property(e => e.PayPlanType)
                .HasMaxLength(5)
                .HasColumnName("PAY_PLAN_TYPE");
            entity.Property(e => e.RateTableDesc)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("RATE_TABLE_DESC");
        });

        modelBuilder.Entity<ConfigPayplanType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CONFIG_PAYPLAN_TYPE");

            entity.Property(e => e.DefaultRateTable).HasColumnName("DEFAULT_RATE_TABLE");
            entity.Property(e => e.PayPlanDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAY_PLAN_DESC");
            entity.Property(e => e.PayPlanType)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("PAY_PLAN_TYPE");
        });

        //modelBuilder.Entity<ConfigPerformanceThreshold>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasFillFactor(90);

        //    entity.ToTable("CONFIG_PERFORMANCE_THRESHOLD");

        //    entity.HasIndex(e => new { e.PayPlan, e.EmployeeId }, "UK_CONFIG_PERFORMANCE_THRESHOLD_PayPlanEmployeeID")
        //        .IsUnique()
        //        .HasFillFactor(90);

        //    entity.Property(e => e.Id).HasColumnName("ID");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(60)
        //        .HasColumnName("EmployeeID");
        //    entity.Property(e => e.MinimumAnnualRevenuePercentage)
        //        .HasDefaultValue(100m)
        //        .HasColumnType("numeric(5, 2)");
        //    entity.Property(e => e.PayPlan).HasMaxLength(30);
        //});

        //modelBuilder.Entity<ConfigPerformanceThreshold2016>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_PERFORMANCE_THRESHOLD_2016");

        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(60)
        //        .HasColumnName("EmployeeID");
        //    entity.Property(e => e.Id)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("ID");
        //    entity.Property(e => e.MinimumAnnualRevenuePercentage).HasColumnType("numeric(5, 2)");
        //    entity.Property(e => e.PayPlan).HasMaxLength(30);
        //});

        //modelBuilder.Entity<ConfigPerformanceThresholdPeriodOverride>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasFillFactor(90);

        //    entity.ToTable("CONFIG_PERFORMANCE_THRESHOLD_PERIOD_OVERRIDE");

        //    entity.HasIndex(e => new { e.ConfigPerformanceThresholdId, e.EmployeeId, e.Year, e.Period }, "UK_CONFIG_PERFORMANCE_THRESHOLD_PERIOD_OVERRIDE_ThresholdEmployeeIDPeriod")
        //        .IsUnique()
        //        .HasFillFactor(90);

        //    entity.Property(e => e.Id).HasColumnName("ID");
        //    entity.Property(e => e.ConfigPerformanceThresholdId).HasColumnName("CONFIG_PERFORMANCE_THRESHOLD_ID");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(60)
        //        .HasColumnName("EmployeeID");
        //    entity.Property(e => e.RevenueTarget).HasColumnType("numeric(18, 2)");
        //});

        modelBuilder.Entity<ConfigProcessingType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CONFIG_PROCESSING_TYPE");

            entity.Property(e => e.Payplan)
                .HasMaxLength(2)
                .HasColumnName("payplan");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasColumnName("periodYear");
            entity.Property(e => e.ProcessingType)
                .HasMaxLength(10)
                .HasColumnName("processingType");
        });

        //modelBuilder.Entity<ConfigPromoUplift>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_PROMO_UPLIFT");

        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasColumnName("period_month");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(5)
        //        .HasColumnName("period_year");
        //    entity.Property(e => e.PromoCode)
        //        .HasMaxLength(10)
        //        .HasColumnName("promo_code");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //    entity.Property(e => e.Uplift)
        //        .HasColumnType("numeric(18, 5)")
        //        .HasColumnName("uplift");
        //});

        //modelBuilder.Entity<ConfigRateTableCriterion>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_RATE_TABLE_CRITERIA");

        //    entity.Property(e => e.CriteriaId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("CRITERIA_ID");
        //    entity.Property(e => e.CriteriaType)
        //        .HasMaxLength(150)
        //        .IsUnicode(false)
        //        .HasColumnName("CRITERIA_TYPE");
        //    entity.Property(e => e.CriteriaValue)
        //        .HasMaxLength(150)
        //        .IsUnicode(false)
        //        .HasColumnName("CRITERIA_VALUE");
        //    entity.Property(e => e.RateTableId).HasColumnName("RATE_TABLE_ID");
        //});

        //modelBuilder.Entity<ConfigRateTableValue>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("CONFIG_RATE_TABLE_VALUES");

        //    entity.HasIndex(e => new { e.RateTableId, e.OrderDiscountMin, e.OrderDiscountMax, e.RateValue }, "IX_UNIQUE_CONFIG_RATE_TABLE_VALUES")
        //        .IsUnique()
        //        .IsClustered()
        //        .HasFillFactor(90);

        //    entity.Property(e => e.OrderDiscountMax)
        //        .HasColumnType("numeric(18, 2)")
        //        .HasColumnName("ORDER_DISCOUNT_MAX");
        //    entity.Property(e => e.OrderDiscountMin)
        //        .HasColumnType("numeric(18, 2)")
        //        .HasColumnName("ORDER_DISCOUNT_MIN");
        //    entity.Property(e => e.RateTableId).HasColumnName("RATE_TABLE_ID");
        //    entity.Property(e => e.RateValue)
        //        .HasColumnType("numeric(18, 3)")
        //        .HasColumnName("RATE_VALUE");
        //});

        modelBuilder.Entity<ConfigRollupType>(entity =>
        {
            entity.HasKey(e => e.RollupTypeId)
                .HasName("PK_ROLLUP_TYPE_ID")
                .HasFillFactor(90);

            entity.ToTable("CONFIG_ROLLUP_TYPE");

            entity.Property(e => e.RollupTypeId).HasColumnName("ROLLUP_TYPE_ID");
            entity.Property(e => e.PayplanType)
                .HasMaxLength(5)
                .HasColumnName("PAYPLAN_TYPE");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
                .HasColumnName("PERIOD_YEAR");
            entity.Property(e => e.RollupCriteria).HasColumnName("ROLLUP_CRITERIA");
            entity.Property(e => e.RollupType)
                .HasMaxLength(30)
                .HasColumnName("ROLLUP_TYPE");
        });

        modelBuilder.Entity<ConfigSalesPeriod>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CONFIG_SALES_PERIOD");

            entity.Property(e => e.PeriodEnd)
                .HasColumnType("datetime")
                .HasColumnName("PERIOD_END");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodStart)
                .HasColumnType("datetime")
                .HasColumnName("PERIOD_START");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("PERIOD_YEAR");
        });

        modelBuilder.Entity<ConfigSystemParameter>(entity =>
        {
            entity.HasKey(e => e.ParameterName);
            entity.ToTable("CONFIG_SYSTEM_PARAMETERS");

            entity.HasIndex(e => e.ParameterName, "IX_CSP_PARAMETER_NAME").HasFillFactor(90);

            entity.HasIndex(e => new { e.ParameterName, e.ParameterValue }, "_unique_CONFIG_SYSTEM_PARAMETERS")
                .IsUnique()
                .HasFillFactor(90);

            entity.Property(e => e.ParameterDescription)
                .HasMaxLength(500)
                .HasColumnName("PARAMETER_DESCRIPTION");
            entity.Property(e => e.ParameterName)
                .HasMaxLength(50)
                .HasColumnName("PARAMETER_NAME");
            entity.Property(e => e.ParameterValue)
                .HasMaxLength(50)
                .HasColumnName("PARAMETER_VALUE");
        });

        modelBuilder.Entity<DataBusinessUnit>(entity =>
        {
            entity.HasKey(e => e.DataBusinessUnitId)
                .HasName("PK_DATA_BUSINESS_UNIT_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_BUSINESS_UNIT");

            entity.HasIndex(e => e.BuName, "IX_DBU_BU_NAME").HasFillFactor(90);

            entity.HasIndex(e => e.OrderId, "IX_DBU_ORDER_ID").HasFillFactor(90);

            entity.Property(e => e.DataBusinessUnitId).HasColumnName("DATA_BUSINESS_UNIT_ID");
            entity.Property(e => e.BuName)
                .HasMaxLength(100)
                .HasColumnName("BU_NAME");
            entity.Property(e => e.OrderId)
                .HasMaxLength(15)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.StatusCd)
                .HasMaxLength(30)
                .HasColumnName("STATUS_CD");
            entity.Property(e => e.StatusDt)
                .HasColumnType("datetime")
                .HasColumnName("STATUS_DT");
        });

        modelBuilder.Entity<DataCancelReplaceSkippedOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DATA_CANCEL_REPLACE_SKIPPED_ORDER");

            entity.Property(e => e.DataCancelReplaceSkippedOrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DATA_CANCEL_REPLACE_SKIPPED_ORDER_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(30)
                .HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.OrderRowId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ROW_ID");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(2)
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasColumnName("PERIOD_YEAR");
        });

        //modelBuilder.Entity<DataCommisionPaymentExcludeFromPerformanceDeduction>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_COMMISION_PAYMENT_EXCLUDE_FROM_PERFORMANCE_DEDUCTION");

        //    entity.Property(e => e.Created)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("created");
        //    entity.Property(e => e.DataCommissionPaymentRowId).HasColumnName("data_commission_payment_row_id");
        //    entity.Property(e => e.Reason)
        //        .HasMaxLength(100)
        //        .HasColumnName("reason");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //});

        //modelBuilder.Entity<DataCommisionPaymentThresholdReleased>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_COMMISION_PAYMENT_THRESHOLD_RELEASED");

        //    entity.Property(e => e.OriginalDataCommissionPaymentId).HasColumnName("ORIGINAL_DATA_COMMISSION_PAYMENT_ID");
        //    entity.Property(e => e.OriginalPeriodMonth)
        //        .HasMaxLength(2)
        //        .HasDefaultValue("00")
        //        .HasColumnName("ORIGINAL_PERIOD_MONTH");
        //    entity.Property(e => e.OriginalPeriodYear)
        //        .HasMaxLength(5)
        //        .HasDefaultValue("00000")
        //        .HasColumnName("ORIGINAL_PERIOD_YEAR");
        //    entity.Property(e => e.ReleasePeriodMonth)
        //        .HasMaxLength(2)
        //        .HasDefaultValue("00")
        //        .HasColumnName("RELEASE_PERIOD_MONTH");
        //    entity.Property(e => e.ReleasesPeriodYear)
        //        .HasMaxLength(5)
        //        .HasDefaultValue("00000")
        //        .HasColumnName("RELEASES_PERIOD_YEAR");
        //    entity.Property(e => e.ThresholdReleaseId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("THRESHOLD_RELEASE_ID");
        //});

        modelBuilder.Entity<DataCommissionPayment>(entity =>
        {
            entity.HasKey(e => e.DataCommissionPaymentId)
                .HasName("PK_DATA_COMMISSION_PAYMENT_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_COMMISSION_PAYMENT");

            entity.HasIndex(e => new { e.PeriodYear, e.PeriodMonth, e.PaymentSource, e.PaymentValue }, "IX_DCP_PAYMENT_SOURCE_VALUES").HasFillFactor(90);

            entity.Property(e => e.DataCommissionPaymentId).HasColumnName("DATA_COMMISSION_PAYMENT_ID");
            entity.Property(e => e.AdjustmentId).HasColumnName("ADJUSTMENT_ID");
            //entity.Property(e => e.CreditAllocationId).HasColumnName("CREDIT_ALLOCATION_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(30)
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.EnhancementId).HasColumnName("ENHANCEMENT_ID");
            entity.Property(e => e.OrderId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.PaymentDescription)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_DESCRIPTION");
            entity.Property(e => e.PaymentRate)
                .HasColumnType("numeric(18, 7)")
                .HasColumnName("PAYMENT_RATE");
            entity.Property(e => e.PaymentSource)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_SOURCE");
            entity.Property(e => e.PaymentValue)
                .HasColumnType("numeric(18, 7)")
                .HasColumnName("PAYMENT_VALUE");
            entity.Property(e => e.PaymentWitheld).HasColumnName("PAYMENT_WITHELD");
            entity.Property(e => e.PayplanRateId).HasColumnName("PAYPLAN_RATE_ID");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(2)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
                .HasColumnName("PERIOD_YEAR");
            entity.Property(e => e.PositionId)
                .HasMaxLength(15)
                .HasColumnName("POSITION_ID");
        });

        //modelBuilder.Entity<DataCommissionPaymentOverride>(entity =>
        //{
        //    entity.HasKey(e => e.DataCommissionPaymentOverrideId).HasFillFactor(90);

        //    entity.ToTable("DATA_COMMISSION_PAYMENT_OVERRIDE");

        //    entity.Property(e => e.DataCommissionPaymentOverrideId).HasColumnName("DATA_COMMISSION_PAYMENT_OVERRIDE_ID");
        //    entity.Property(e => e.DataCommissionPaymentId).HasColumnName("DATA_COMMISSION_PAYMENT_ID");
        //    entity.Property(e => e.Release).HasColumnName("RELEASE");
        //});

        //modelBuilder.Entity<DataCommissionRateMatrix>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_COMMISSION_RATE_MATRIX");

        //    entity.Property(e => e.CommissionRateMatrixId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("commission_rate_matrix_id");
        //    entity.Property(e => e.CommissionValue)
        //        .HasColumnType("decimal(5, 2)")
        //        .HasColumnName("commission_value");
        //    entity.Property(e => e.CssTargetPercentageEnd).HasColumnName("css_target_percentage_end");
        //    entity.Property(e => e.CssTargetPercentageStart).HasColumnName("css_target_percentage_start");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(50)
        //        .HasColumnName("employee_id");
        //    entity.Property(e => e.MsTargetPercentageEnd).HasColumnName("ms_target_percentage_end");
        //    entity.Property(e => e.MsTargetPercentageStart).HasColumnName("ms_target_percentage_start");
        //    entity.Property(e => e.Payplan)
        //        .HasMaxLength(5)
        //        .HasColumnName("payplan");
        //    entity.Property(e => e.PeriodMonthEnd).HasColumnName("period_month_end");
        //    entity.Property(e => e.PeriodMonthStart).HasColumnName("period_month_start");
        //    entity.Property(e => e.PeriodYear).HasColumnName("period_year");
        //});

        //modelBuilder.Entity<DataCommissionsMultiplier>(entity =>
        //{
        //    entity.HasKey(e => e.DataCommissionsMultiplierId).HasFillFactor(90);

        //    entity.ToTable("DATA_COMMISSIONS_MULTIPLIER");

        //    entity.Property(e => e.DataCommissionsMultiplierId).HasColumnName("DATA_COMMISSIONS_MULTIPLIER_ID");
        //    entity.Property(e => e.Active).HasColumnName("ACTIVE");
        //    entity.Property(e => e.Created)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("CREATED");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasDefaultValue("X")
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.FinYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("FIN_YEAR");
        //    entity.Property(e => e.PayPlanType)
        //        .HasMaxLength(5)
        //        .HasDefaultValue("XXXXX")
        //        .HasColumnName("PAY_PLAN_TYPE");
        //    entity.Property(e => e.PositionId)
        //        .HasMaxLength(30)
        //        .HasDefaultValue("X")
        //        .HasColumnName("POSITION_ID");
        //    entity.Property(e => e.Updated)
        //        .HasColumnType("datetime")
        //        .HasColumnName("UPDATED");
        //});

        modelBuilder.Entity<DataCreditAllocation>(entity =>
        {
            entity.HasKey(e => e.DataCreditAllocationId)
                .HasName("PK_DATA_CREDIT_ALLOCATION_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_CREDIT_ALLOCATION");

            entity.HasIndex(e => new { e.EmployeeId, e.AllocationTypeId, e.PeriodMonth, e.PeriodYear, e.ExcludeFromCalcs, e.AllocationList, e.AllocationValue }, "IX_DCA_ACHIEVEMENT").HasFillFactor(90);

            entity.HasIndex(e => e.AdjustmentId, "IX_DCA_ADJUSTMENT_ID").HasFillFactor(90);

            entity.HasIndex(e => e.AllocationSource, "IX_DCA_ALLOCATION_SOURCE").HasFillFactor(90);

            entity.HasIndex(e => e.AllocationTypeId, "IX_DCA_ALLOCATION_TYPE_ID").HasFillFactor(90);

            entity.HasIndex(e => new { e.AllocationTypeId, e.PeriodMonth, e.PeriodYear }, "IX_DCA_ALLOCATION_TYPE_ID_PERIOD_MONTH_PERIOD_YEAR").HasFillFactor(90);

            entity.HasIndex(e => new { e.EmployeeId, e.AllocationTypeId, e.AllocationValue, e.PeriodMonth, e.PeriodYear, e.ExcludeFromCalcs, e.PositionId }, "IX_DCA_EMPLOYEE_ID").HasFillFactor(90);

            entity.HasIndex(e => e.ItemId, "IX_DCA_ITEM_ID").HasFillFactor(90);

            entity.HasIndex(e => new { e.AllocationTypeId, e.OrderId, e.EmployeeId, e.PositionId }, "IX_DCA_ORDERS_EMPLOYEES").HasFillFactor(90);

            entity.HasIndex(e => e.OrderId, "IX_DCA_ORDER_ID").HasFillFactor(90);

            entity.HasIndex(e => e.PeriodYear, "IX_DCA_PERIOD_YEAR").HasFillFactor(90);

            entity.HasIndex(e => e.PositionId, "IX_DCA_POSITION_ID").HasFillFactor(90);

            entity.HasIndex(e => new { e.RollupProcessed, e.DataCreditAllocationId }, "IX_DCA_ROLLUP").HasFillFactor(90);

            entity.Property(e => e.DataCreditAllocationId).HasColumnName("DATA_CREDIT_ALLOCATION_ID");
            entity.Property(e => e.AdjustmentId).HasColumnName("ADJUSTMENT_ID");
            entity.Property(e => e.AllocationList)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ALLOCATION_LIST");
            entity.Property(e => e.AllocationSource)
                .HasMaxLength(30)
                .HasColumnName("ALLOCATION_SOURCE");
            entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
            entity.Property(e => e.AllocationValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ALLOCATION_VALUE");
            entity.Property(e => e.ChildPositionId)
                .HasMaxLength(30)
                .HasColumnName("CHILD_POSITION_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(30)
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.ExcludeFromCalcs)
                .HasMaxLength(1)
                .HasColumnName("EXCLUDE_FROM_CALCS");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ITEM_ID");
            entity.Property(e => e.OrderId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(2)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
                .HasColumnName("PERIOD_YEAR");
            entity.Property(e => e.PositionId)
                .HasMaxLength(30)
                .HasColumnName("POSITION_ID");
            entity.Property(e => e.RollupProcessed)
                .HasMaxLength(1)
                .HasDefaultValue("N")
                .HasColumnName("ROLLUP_PROCESSED");
        });

        //modelBuilder.Entity<DataCutoffOrder>(entity =>
        //{
        //    entity.HasKey(e => e.DataCutoffOrdersId)
        //        .HasName("PK_DATA_CUTOFF_ORDERS_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("DATA_CUTOFF_ORDERS");

        //    entity.Property(e => e.DataCutoffOrdersId).HasColumnName("DATA_CUTOFF_ORDERS_ID");
        //    entity.Property(e => e.CutoffDt)
        //        .HasColumnType("datetime")
        //        .HasColumnName("CUTOFF_DT");
        //    entity.Property(e => e.DateCreated)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("DATE_CREATED");
        //    entity.Property(e => e.OrderDt)
        //        .HasColumnType("datetime")
        //        .HasColumnName("ORDER_DT");
        //    entity.Property(e => e.OrderId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ORDER_ID");
        //    entity.Property(e => e.OrderNumber)
        //        .HasMaxLength(30)
        //        .HasColumnName("ORDER_NUMBER");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
        //        .HasColumnName("PERIOD_MONTH");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("PERIOD_YEAR");
        //    entity.Property(e => e.StatusCd)
        //        .HasMaxLength(200)
        //        .HasColumnName("STATUS_CD");
        //    entity.Property(e => e.StatusDt)
        //        .HasColumnType("datetime")
        //        .HasColumnName("STATUS_DT");
        //});

        modelBuilder.Entity<DataDefaultTarget>(entity =>
        {
            entity.HasKey(e => e.DataDefaultTargetsId)
                .HasName("PK_DATA_DEFAULT_TARGETS_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_DEFAULT_TARGETS");

            entity.Property(e => e.DataDefaultTargetsId).HasColumnName("DATA_DEFAULT_TARGETS_ID");
            entity.Property(e => e.AllocationDescription)
                .HasMaxLength(500)
                .HasColumnName("ALLOCATION_DESCRIPTION");
            entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(30)
                .HasDefaultValue("X")
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.FinYear)
                .HasMaxLength(4)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
                .HasColumnName("FIN_YEAR");
            entity.Property(e => e.PayPlanType)
                .HasMaxLength(5)
                .HasColumnName("PAY_PLAN_TYPE");
            entity.Property(e => e.TargetValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("TARGET_VALUE");
        });

        modelBuilder.Entity<DataEmployee>(entity =>
        {
            entity.HasKey(e => e.DataEmployeesId)
                .HasName("PK_DATA_EMPLOYEES_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_EMPLOYEES");

            entity.HasIndex(e => e.RowId, "idx_DATA_EMPLOYEES_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.DataEmployeesId).HasColumnName("DATA_EMPLOYEES_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.DeleteFlag)
                .HasDefaultValue(0)
                .HasComment("Indicates whether EMPLOYEE has been deleted from the imported TMP_DATA_EMPLOYEES extract [1 = DELETED]")
                .HasColumnName("DELETE_FLAG");
            entity.Property(e => e.EmailAddr)
                .HasMaxLength(200)
                .HasColumnName("EMAIL_ADDR");
            entity.Property(e => e.EmployeeNumber)
                .HasMaxLength(100)
                .HasColumnName("EMPLOYEE_NUMBER");
            entity.Property(e => e.FstName)
                .HasMaxLength(100)
                .HasColumnName("FST_NAME");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(150)
                .HasColumnName("JOB_TITLE");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("LOGIN");
            entity.Property(e => e.PrHeldPostnId)
                .HasMaxLength(30)
                .HasColumnName("PR_HELD_POSTN_ID");
            entity.Property(e => e.RowId)
                .HasMaxLength(30)
                .HasColumnName("ROW_ID");
        });

        modelBuilder.Entity<DataEmployeeTarget>(entity =>
        {
            entity.HasKey(e => e.DataEmployeeTargetsId).HasFillFactor(90);

            entity.ToTable("DATA_EMPLOYEE_TARGETS");

            entity.Property(e => e.DataEmployeeTargetsId).HasColumnName("DATA_EMPLOYEE_TARGETS_ID");
            entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(30)
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.Payplan)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("PAYPLAN");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(2)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
                .HasColumnName("PERIOD_YEAR");
            entity.Property(e => e.TargetValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("TARGET_VALUE");
        });

        modelBuilder.Entity<DataExcludedProduct>(entity =>
        {
            entity.HasKey(e => e.DataExcludedProductsId);

            entity.ToTable("DATA_EXCLUDED_PRODUCTS");

            entity.Property(e => e.DataExcludedProductsId).ValueGeneratedOnAdd().HasColumnName("DATA_EXCLUDED_PRODUCTS_ID");
            entity.Property(e => e.Excluded)
                .HasDefaultValue(1)
                .HasColumnName("EXCLUDED");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ITEM_ID");
            entity.Property(e => e.LevelBased)
                .HasDefaultValue(0)
                .HasColumnName("LEVEL_BASED");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(30)
                .HasColumnName("PRODUCT_CODE");
            entity.Property(e => e.ProductDesc)
                .HasMaxLength(500)
                .HasColumnName("PRODUCT_DESC");
            entity.Property(e => e.ProductLevel1)
                .HasMaxLength(20)
                .HasColumnName("PRODUCT_LEVEL_1");
            entity.Property(e => e.ProductLevel2)
                .HasMaxLength(20)
                .HasColumnName("PRODUCT_LEVEL_2");
            entity.Property(e => e.ProductLevel3)
                .HasMaxLength(20)
                .HasColumnName("PRODUCT_LEVEL_3");
            entity.Property(e => e.ProductType)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_TYPE");
        });

        //modelBuilder.Entity<DataFollowUpEntitlement>(entity =>
        //{
        //    entity.HasKey(e => e.DataFollowUpEntitlementsId)
        //        .HasName("PK_DATA_FOLLOW_UP_ENTITLEMENTS_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("DATA_FOLLOW_UP_ENTITLEMENTS");

        //    entity.HasIndex(e => e.CustomerAccountNumber, "IX_DFUE_CUSTOMER_ACCOUNT_NUMBER").HasFillFactor(90);

        //    entity.HasIndex(e => e.OrderRowId, "IX_DFUE_ORDER_ROW_ID").HasFillFactor(90);

        //    entity.Property(e => e.DataFollowUpEntitlementsId).HasColumnName("DATA_FOLLOW_UP_ENTITLEMENTS_ID");
        //    entity.Property(e => e.CustomerAccountNumber)
        //        .HasMaxLength(60)
        //        .HasColumnName("CUSTOMER_ACCOUNT_NUMBER");
        //    entity.Property(e => e.DateCreated)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("DATE_CREATED");
        //    entity.Property(e => e.OrderRowId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ORDER_ROW_ID");
        //    entity.Property(e => e.PositionRowId)
        //        .HasMaxLength(30)
        //        .HasColumnName("POSITION_ROW_ID");
        //    entity.Property(e => e.PrEmpId)
        //        .HasMaxLength(30)
        //        .HasColumnName("PR_EMP_ID");
        //    entity.Property(e => e.Rate)
        //        .HasColumnType("numeric(2, 0)")
        //        .HasColumnName("RATE");
        //    entity.Property(e => e.ValidFrom)
        //        .HasColumnType("datetime")
        //        .HasColumnName("VALID_FROM");
        //    entity.Property(e => e.ValidTo)
        //        .HasColumnType("datetime")
        //        .HasColumnName("VALID_TO");
        //});

        //modelBuilder.Entity<DataFollowUpPayment>(entity =>
        //{
        //    entity.HasKey(e => e.DataFollowUpPaymentsId)
        //        .HasName("PK_DATA_FOLLOW_UP_PAYMENTS_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("DATA_FOLLOW_UP_PAYMENTS");

        //    entity.Property(e => e.DataFollowUpPaymentsId).HasColumnName("DATA_FOLLOW_UP_PAYMENTS_ID");
        //    entity.Property(e => e.DateCreated)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("DATE_CREATED");
        //    entity.Property(e => e.EntOrderRowId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ENT_ORDER_ROW_ID");
        //    entity.Property(e => e.OrderRowId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ORDER_ROW_ID");
        //});

        //modelBuilder.Entity<DataImportOverride>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_IMPORT_OVERRIDE");

        //    entity.Property(e => e.RowId)
        //        .HasMaxLength(50)
        //        .HasColumnName("ROW_ID");
        //});

        modelBuilder.Entity<DataImpressCommissionIncident>(entity =>
        {
            entity.HasKey(e => e.DataImpressCommissionIncidentId)
                .HasName("DATA_IMPRESS_COMMISSION_INCIDENT_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_IMPRESS_COMMISSION_INCIDENT");

            entity.Property(e => e.DataImpressCommissionIncidentId).HasColumnName("DATA_IMPRESS_COMMISSION_INCIDENT_ID");
            entity.Property(e => e.IncidentDescription)
                .HasMaxLength(500)
                .HasColumnName("INCIDENT_DESCRIPTION");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(4)
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasColumnName("PERIOD_YEAR");
            entity.Property(e => e.SfEmployeeId)
                .HasMaxLength(30)
                .HasColumnName("SF_EMPLOYEE_ID");
            entity.Property(e => e.SfOrderId)
                .HasMaxLength(15)
                .HasColumnName("SF_ORDER_ID");
        });

        modelBuilder.Entity<DataImpressSubscriptionOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DATA_IMPRESS_SUBSCRIPTION_ORDER");

            entity.Property(e => e.DataImpressSubscriptionOrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DATA_IMPRESS_SUBSCRIPTION_ORDER_ID");
            entity.Property(e => e.ImAccountName)
                .HasMaxLength(50)
                .HasColumnName("IM_ACCOUNT_NAME");
            entity.Property(e => e.ImAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("IM_ACCOUNT_NUMBER");
            entity.Property(e => e.ImActualUsage).HasColumnName("IM_ACTUAL_USAGE");
            entity.Property(e => e.ImOrderNumber)
                .HasMaxLength(50)
                .HasColumnName("IM_ORDER_NUMBER");
            entity.Property(e => e.ImPlanName)
                .HasMaxLength(50)
                .HasColumnName("IM_PLAN_NAME");
            entity.Property(e => e.ImPlanTier)
                .HasMaxLength(50)
                .HasColumnName("IM_PLAN_TIER");
            entity.Property(e => e.ImPricePerUsageItem)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("IM_PRICE_PER_USAGE_ITEM");
            entity.Property(e => e.Processed).HasColumnName("PROCESSED");
            entity.Property(e => e.SfAnticipatedUsage)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("SF_ANTICIPATED_USAGE");
            entity.Property(e => e.SfEmployeeId)
                .HasMaxLength(50)
                .HasColumnName("SF_EMPLOYEE_ID");
            entity.Property(e => e.SfLastStatusUpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("SF_LAST_STATUS_UPDATE_TIME");
            entity.Property(e => e.SfOrderNumber)
                .HasMaxLength(50)
                .HasColumnName("SF_ORDER_NUMBER");
            entity.Property(e => e.SfOrderStatus)
                .HasMaxLength(150)
                .HasColumnName("SF_ORDER_STATUS");
            entity.Property(e => e.SfSubscriptionMonthsAmount).HasColumnName("SF_SUBSCRIPTION_MONTHS_AMOUNT");
            entity.Property(e => e.SfSubscriptionPrice)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("SF_SUBSCRIPTION_PRICE");
        });

        modelBuilder.Entity<DataManualAdjustment>(entity =>
        {
            entity.HasKey(e => e.AdjustmentId)
                .HasName("PK_ADJUSTMENTS_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_MANUAL_ADJUSTMENTS");

            entity.Property(e => e.AdjustmentId).HasColumnName("ADJUSTMENT_ID");
            entity.Property(e => e.AdjustmentBy)
                .HasMaxLength(100)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("ADJUSTMENT_BY");
            entity.Property(e => e.AdjustmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ADJUSTMENT_DATE");
            entity.Property(e => e.AdjustmentProcessed)
                .HasMaxLength(1)
                .HasDefaultValue("N")
                .HasColumnName("ADJUSTMENT_PROCESSED");
            entity.Property(e => e.AdjustmentReason).HasColumnName("ADJUSTMENT_REASON");
            entity.Property(e => e.AdjustmentType)
                .HasMaxLength(20)
                .HasColumnName("ADJUSTMENT_TYPE");
            entity.Property(e => e.AllocationList)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ALLOCATION_LIST");
            entity.Property(e => e.AllocationRate)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ALLOCATION_RATE");
            entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
            entity.Property(e => e.AllocationValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ALLOCATION_VALUE");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(30)
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.ExcludeFromCalcs)
                .HasMaxLength(1)
                .HasColumnName("EXCLUDE_FROM_CALCS");
            entity.Property(e => e.ItIssue).HasColumnName("IT_ISSUE");
            entity.Property(e => e.OrderId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(60)
                .HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(2)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
                .HasColumnName("PERIOD_YEAR");
            entity.Property(e => e.PositionId)
                .HasMaxLength(30)
                .HasColumnName("POSITION_ID");
        });

        //modelBuilder.Entity<DataMatrixCommissionRate>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_MATRIX_COMMISSION_RATES");

        //    entity.Property(e => e.CommPct)
        //        .HasColumnType("numeric(20, 5)")
        //        .HasColumnName("comm_pct");
        //    entity.Property(e => e.CssAllocId).HasColumnName("css_alloc_id");
        //    entity.Property(e => e.CssMthTarget)
        //        .HasColumnType("numeric(20, 5)")
        //        .HasColumnName("css_mth_target");
        //    entity.Property(e => e.CssPct)
        //        .HasColumnType("numeric(20, 5)")
        //        .HasColumnName("css_pct");
        //    entity.Property(e => e.CssRevenue)
        //        .HasDefaultValue(0m)
        //        .HasColumnType("numeric(20, 5)")
        //        .HasColumnName("css_revenue");
        //    entity.Property(e => e.EmpId)
        //        .HasMaxLength(50)
        //        .HasColumnName("emp_id");
        //    entity.Property(e => e.MsAllocId).HasColumnName("ms_alloc_id");
        //    entity.Property(e => e.MsMthTarget)
        //        .HasColumnType("numeric(20, 5)")
        //        .HasColumnName("ms_mth_target");
        //    entity.Property(e => e.MsPct)
        //        .HasColumnType("numeric(20, 5)")
        //        .HasColumnName("ms_pct");
        //    entity.Property(e => e.MsRevenue)
        //        .HasDefaultValue(0m)
        //        .HasColumnType("numeric(20, 5)")
        //        .HasColumnName("ms_revenue");
        //    entity.Property(e => e.Payplan)
        //        .HasMaxLength(5)
        //        .HasColumnName("payplan");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasColumnName("period_month");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(5)
        //        .HasColumnName("period_year");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //});

        //modelBuilder.Entity<DataNegativeCommission>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_NEGATIVE_COMMISSION");

        //    entity.Property(e => e.CommissionValue)
        //        .HasColumnType("numeric(18, 7)")
        //        .HasColumnName("commissionValue");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasColumnName("employeeId");
        //    entity.Property(e => e.OrderId)
        //        .HasMaxLength(30)
        //        .HasColumnName("orderId");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //});

        modelBuilder.Entity<DataNewcomerSetting>(entity =>
        {
            entity.HasKey(e => e.DataNewcomerSettingsId)
                .HasName("DATA_NEWCOMER_SETTINGS_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_NEWCOMER_SETTINGS");

            entity.Property(e => e.DataNewcomerSettingsId).HasColumnName("DATA_NEWCOMER_SETTINGS_ID");
            entity.Property(e => e.Activated)
                .HasDefaultValue(0)
                .HasColumnName("ACTIVATED");
            entity.Property(e => e.EmployeeRowId)
                .HasMaxLength(15)
                .HasColumnName("EMPLOYEE_ROW_ID");
            entity.Property(e => e.Guarantee)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("GUARANTEE");
            entity.Property(e => e.PeriodDurationInMonths).HasColumnName("PERIOD_DURATION_IN_MONTHS");
            entity.Property(e => e.PeriodStartDate)
                .HasColumnType("datetime")
                .HasColumnName("PERIOD_START_DATE");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .HasColumnName("POSITION_NAME");
        });

        modelBuilder.Entity<DataOrderHeader>(entity =>
        {
            entity.HasKey(e => e.DataOrderHeadersId)
                .HasName("PK_DATA_ORDER_HEADERS_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_ORDER_HEADERS");

            entity.HasIndex(e => e.CustomerAccountNumber, "IX_DATA_ORDER_HEADERS_CUSTOMER_ACCOUNT_NUMBER").HasFillFactor(90);

            entity.HasIndex(e => e.DateCreated, "IX_DATA_ORDER_HEADERS_DATE_CREATED").HasFillFactor(90);

            entity.HasIndex(e => e.OrderNumber, "IX_DATA_ORDER_HEADERS_ORDER_NUMBER").HasFillFactor(90);

            entity.HasIndex(e => e.OrderRowId, "IX_DATA_ORDER_HEADERS_ORDER_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.DataOrderHeadersId).HasColumnName("DATA_ORDER_HEADERS_ID");
            entity.Property(e => e.CustomerAccountNumber)
                .HasMaxLength(60)
                .HasColumnName("CUSTOMER_ACCOUNT_NUMBER");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .HasColumnName("CUSTOMER_NAME");
            entity.Property(e => e.CustomerType)
                .HasMaxLength(60)
                .HasColumnName("CUSTOMER_TYPE");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.MaintenanceTerm)
                .HasMaxLength(50)
                .HasColumnName("MAINTENANCE_TERM");
            entity.Property(e => e.OrderDiscountPercent)
                .HasComputedColumnSql("([dbo].[ufn_GetDiscountPct]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ORDER_DISCOUNT_PERCENT");
            entity.Property(e => e.OrderDiscountVal)
                .HasComputedColumnSql("([dbo].[ufn_GetDiscountVal]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ORDER_DISCOUNT_VAL");
            entity.Property(e => e.OrderListVal)
                .HasComputedColumnSql("([dbo].[ufn_GetListVal]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ORDER_LIST_VAL");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(60)
                .HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.OrderRowId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ROW_ID");
            entity.Property(e => e.OrderSaleVal)
                .HasComputedColumnSql("([dbo].[ufn_GetSaleVal]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ORDER_SALE_VAL");
            entity.Property(e => e.OrderType)
                .HasMaxLength(100)
                .HasColumnName("ORDER_TYPE");
            entity.Property(e => e.PeriodMonth)
                .HasMaxLength(2)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
                .HasColumnName("PERIOD_MONTH");
            entity.Property(e => e.PeriodYear)
                .HasMaxLength(4)
                .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
                .HasColumnName("PERIOD_YEAR");
            entity.Property(e => e.PrPostnId)
                .HasMaxLength(30)
                .HasColumnName("PR_POSTN_ID");
            entity.Property(e => e.PromotionCode)
                .HasMaxLength(60)
                .HasColumnName("PROMOTION_CODE");
            entity.Property(e => e.SapOrderReference)
                .HasMaxLength(20)
                .HasColumnName("SAP_ORDER_REFERENCE");
            entity.Property(e => e.ServiceDiscountPercent)
                .HasComputedColumnSql("([dbo].[ufn_GetServiceDiscPct]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("SERVICE_DISCOUNT_PERCENT");
            entity.Property(e => e.ServiceDiscountVal)
                .HasComputedColumnSql("([dbo].[ufn_GetServiceDiscVal]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("SERVICE_DISCOUNT_VAL");
            entity.Property(e => e.ServiceListVal)
                .HasComputedColumnSql("([dbo].[ufn_GetServiceListVal]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("SERVICE_LIST_VAL");
            entity.Property(e => e.ServiceSaleVal)
                .HasComputedColumnSql("([dbo].[ufn_GetServiceSaleVal]([ORDER_ROW_ID]))", false)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("SERVICE_SALE_VAL");
            entity.Property(e => e.ServiceType)
                .HasMaxLength(30)
                .HasComputedColumnSql("([dbo].[ufn_getServiceType]([ORDER_ROW_ID]))", false)
                .HasColumnName("SERVICE_TYPE");
        });

        modelBuilder.Entity<DataOrderItem>(entity =>
        {
            entity.HasKey(e => e.DataOrderItemsId)
                .HasName("PK_DATA_ORDER_ITEMS_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_ORDER_ITEMS");

            entity.HasIndex(e => e.OrderItemId, "IDX_DATA_ORDER_ITEMS_ORDER_ITEM_ID").HasFillFactor(90);

            entity.HasIndex(e => e.ProductCode, "IDX_DATA_ORDER_ITEMS_PRODUCT_CODE").HasFillFactor(90);

            entity.HasIndex(e => e.ProductLevel1, "IDX_DATA_ORDER_ITEMS_PRODUCT_LEVEL_1").HasFillFactor(90);

            entity.HasIndex(e => e.ProductLevel2, "IDX_DATA_ORDER_ITEMS_PRODUCT_LEVEL_2").HasFillFactor(90);

            entity.HasIndex(e => e.ProductLevel3, "IDX_DATA_ORDER_ITEMS_PRODUCT_LEVEL_3").HasFillFactor(90);

            entity.HasIndex(e => e.OrderRowId, "IX_DATA_ORDER_HEADERS_ORDER_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.DataOrderItemsId).HasColumnName("DATA_ORDER_ITEMS_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.LineNumber).HasColumnName("LINE_NUMBER");
            entity.Property(e => e.ListValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("LIST_VALUE");
            entity.Property(e => e.OrderItemId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ITEM_ID");
            entity.Property(e => e.OrderRowId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ROW_ID");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(200)
                .HasColumnName("PRODUCT_CODE");
            entity.Property(e => e.ProductDesc)
                .HasMaxLength(510)
                .HasColumnName("PRODUCT_DESC");
            entity.Property(e => e.ProductLevel1)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_1");
            entity.Property(e => e.ProductLevel2)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_2");
            entity.Property(e => e.ProductLevel3)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_3");
            entity.Property(e => e.ProductType)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_TYPE");
            entity.Property(e => e.Quantity)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.SaleValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("SALE_VALUE");
        });

        modelBuilder.Entity<DataOrderPosition>(entity =>
        {
            entity.HasKey(e => e.DataOrderPositionsId)
                .HasName("PK_DATA_ORDER_POSITIONS_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_ORDER_POSITIONS");

            entity.HasIndex(e => e.OrderRowId, "IDX_DATA_ORDER_POSITIONS_ORDER_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.DataOrderPositionsId).HasColumnName("DATA_ORDER_POSITIONS_ID");
            entity.Property(e => e.AllocationPercentage)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ALLOCATION_PERCENTAGE");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.EmployeeRowId)
                .HasMaxLength(30)
                .HasColumnName("EMPLOYEE_ROW_ID");
            entity.Property(e => e.OrderRowId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ROW_ID");
            entity.Property(e => e.ParentPositionRowId)
                .HasMaxLength(30)
                .HasColumnName("PARENT_POSITION_ROW_ID");
            entity.Property(e => e.PayplanType)
                .HasMaxLength(60)
                .HasColumnName("PAYPLAN_TYPE");
            entity.Property(e => e.PositionName)
                .HasMaxLength(100)
                .HasColumnName("POSITION_NAME");
            entity.Property(e => e.PositionRowId)
                .HasMaxLength(30)
                .HasColumnName("POSITION_ROW_ID");
        });

        modelBuilder.Entity<DataOrderProcessHistory>(entity =>
        {
            entity.HasKey(e => e.DataOrderProcessHistoryId)
                .HasName("PK_DATA_ORDER_PROCESS_HISTORY_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_ORDER_PROCESS_HISTORY");

            entity.HasIndex(e => e.BuName, "idx_DATA_ORDER_PROCESS_HISTORY_BU_NAME").HasFillFactor(90);

            entity.HasIndex(e => e.CommissionProcessed, "idx_DATA_ORDER_PROCESS_HISTORY_COMMISSION_PROCESSED").HasFillFactor(90);

            entity.HasIndex(e => e.RowId, "idx_DATA_ORDER_PROCESS_HISTORY_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.DataOrderProcessHistoryId).HasColumnName("DATA_ORDER_PROCESS_HISTORY_ID");
            entity.Property(e => e.BuName)
                .HasMaxLength(60)
                .HasColumnName("BU_NAME");
            entity.Property(e => e.CommissionProcessed)
                .HasDefaultValue(false)
                .HasColumnName("COMMISSION_PROCESSED");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.RevenueProcessed)
                .HasDefaultValue(false)
                .HasColumnName("REVENUE_PROCESSED");
            entity.Property(e => e.RowId)
                .HasMaxLength(15)
                .HasColumnName("ROW_ID");
        });

        modelBuilder.Entity<DataOrderServiceItem>(entity =>
        {
            entity.HasKey(e => e.DataOrderServiceItemsId)
                .HasName("PK_DATA_ORDER_SERVICE_ITEMS_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_ORDER_SERVICE_ITEMS");

            entity.HasIndex(e => e.OrderRowId, "IX_DATA_ORDER_SERVICE_ITEMS_ORDER_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.DataOrderServiceItemsId).HasColumnName("DATA_ORDER_SERVICE_ITEMS_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.LineNumber).HasColumnName("LINE_NUMBER");
            entity.Property(e => e.ListValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("LIST_VALUE");
            entity.Property(e => e.OrderItemId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ITEM_ID");
            entity.Property(e => e.OrderRowId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ROW_ID");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(200)
                .HasColumnName("PRODUCT_CODE");
            entity.Property(e => e.ProductDesc)
                .HasMaxLength(510)
                .HasColumnName("PRODUCT_DESC");
            entity.Property(e => e.ProductLevel1)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_1");
            entity.Property(e => e.ProductLevel2)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_2");
            entity.Property(e => e.ProductLevel3)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_3");
            entity.Property(e => e.ProductType)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_TYPE");
            entity.Property(e => e.Quantity)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.SaleValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("SALE_VALUE");
        });

        modelBuilder.Entity<DataOrderStatusHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DATA_ORDER_STATUS_HISTORY");

            entity.HasIndex(e => new { e.RowId, e.StatusDt }, "IDX_DATA_ORDER_STATUS_HISTORY_ROW_ID_STATUS_DT")
                .IsClustered()
                .HasFillFactor(90);

            entity.HasIndex(e => e.RowId, "idx_DATA_ORDER_STATUS_HISTORY_ROW_ID").HasFillFactor(90);

            entity.HasIndex(e => e.StatusCd, "idx_DATA_ORDER_STATUS_HISTORY_STATUS_CD").HasFillFactor(90);

            entity.HasIndex(e => e.StatusDt, "idx_DATA_ORDER_STATUS_HISTORY_STATUS_DT").HasFillFactor(90);

            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.RowId)
                .HasMaxLength(15)
                .HasColumnName("ROW_ID");
            entity.Property(e => e.StatusCd)
                .HasMaxLength(200)
                .HasColumnName("STATUS_CD");
            entity.Property(e => e.StatusDt)
                .HasColumnType("datetime")
                .HasColumnName("STATUS_DT");
        });

        //modelBuilder.Entity<DataPayPlanTypeChange>(entity =>
        //{
        //    entity.HasKey(e => e.TypeChangesId)
        //        .HasName("PK_TYPE_CHANGES_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("DATA_PAY_PLAN_TYPE_CHANGES");

        //    entity.Property(e => e.TypeChangesId).HasColumnName("TYPE_CHANGES_ID");
        //    entity.Property(e => e.ChangeDt)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("CHANGE_DT");
        //    entity.Property(e => e.NewPp)
        //        .HasMaxLength(5)
        //        .HasColumnName("NEW_PP");
        //    entity.Property(e => e.OldPp)
        //        .HasMaxLength(5)
        //        .HasColumnName("OLD_PP");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
        //        .HasColumnName("PERIOD_MONTH");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("PERIOD_YEAR");
        //    entity.Property(e => e.Qtr)
        //        .HasMaxLength(2)
        //        .HasColumnName("QTR");
        //    entity.Property(e => e.RepName).HasColumnName("REP_NAME");
        //});

        modelBuilder.Entity<DataPerformanceAgainstTargetMatrix>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DATA_PERFORMANCE_AGAINST_TARGET_MATRIX");

            entity.Property(e => e.AllocationId).HasColumnName("allocationId");
            entity.Property(e => e.CommissionValue)
                .HasColumnType("numeric(18, 5)")
                .HasColumnName("commissionValue");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("employeeId");
            entity.Property(e => e.IsManager)
                .HasDefaultValue(0)
                .HasColumnName("IS_MANAGER");
            entity.Property(e => e.Payplan)
                .HasMaxLength(5)
                .HasColumnName("payplan");
            entity.Property(e => e.PeriodMonthEnd).HasColumnName("periodMonthEnd");
            entity.Property(e => e.PeriodMonthStart).HasColumnName("periodMonthStart");
            entity.Property(e => e.PeriodYear).HasColumnName("periodYear");
            entity.Property(e => e.RowId)
                .ValueGeneratedOnAdd()
                .HasColumnName("rowId");
            entity.Property(e => e.TargetPercentageEnd).HasColumnName("targetPercentageEnd");
            entity.Property(e => e.TargetPercentageStart).HasColumnName("targetPercentageStart");
        });

        //modelBuilder.Entity<DataPerformanceThreshold>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasFillFactor(90);

        //    entity.ToTable("DATA_PERFORMANCE_THRESHOLD");

        //    entity.HasIndex(e => new { e.PeriodYear, e.PeriodMonth, e.EmployeeId, e.PayPlan }, "UK_DATA_PERFORMANCE_THRESHOLD_PeriodEmployeeIDPayPlan")
        //        .IsUnique()
        //        .HasFillFactor(90);

        //    entity.Property(e => e.Id).HasColumnName("ID");
        //    entity.Property(e => e.Employee).HasMaxLength(200);
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(60)
        //        .HasColumnName("EmployeeID");
        //    entity.Property(e => e.PayPlan)
        //        .HasMaxLength(5)
        //        .IsUnicode(false);
        //    entity.Property(e => e.RevenueAchieved).HasColumnType("numeric(18, 2)");
        //    entity.Property(e => e.YtdminimumRevenueTarget)
        //        .HasColumnType("numeric(18, 2)")
        //        .HasColumnName("YTDMinimumRevenueTarget");
        //    entity.Property(e => e.YtdtotalRevenueTarget)
        //        .HasColumnType("numeric(18, 2)")
        //        .HasColumnName("YTDTotalRevenueTarget");
        //});

        modelBuilder.Entity<DataPositionHistory>(entity =>
        {
            entity.HasKey(e => e.DataPositionHistoryId)
                .HasName("PK_DATA_POSITION_HISTORY_ID")
                .HasFillFactor(90);

            entity.ToTable("DATA_POSITION_HISTORY");

            entity.Property(e => e.DataPositionHistoryId).HasColumnName("DATA_POSITION_HISTORY_ID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.EmployeeRowId)
                .HasMaxLength(30)
                .HasColumnName("EMPLOYEE_ROW_ID");
            entity.Property(e => e.EndDt)
                .HasColumnType("datetime")
                .HasColumnName("END_DT");
            entity.Property(e => e.ParentRowId)
                .HasMaxLength(30)
                .HasColumnName("PARENT_ROW_ID");
            entity.Property(e => e.PositionName)
                .HasMaxLength(200)
                .HasColumnName("POSITION_NAME");
            entity.Property(e => e.PositionRowId)
                .HasMaxLength(30)
                .HasColumnName("POSITION_ROW_ID");
            entity.Property(e => e.StartDt)
                .HasColumnType("datetime")
                .HasColumnName("START_DT");
            entity.Property(e => e.XRepType)
                .HasMaxLength(60)
                .HasColumnName("X_REP_TYPE");
        });

        modelBuilder.Entity<DataProductsIncluded>(entity =>
        {
            entity.HasKey(e => e.DataProductsIncludedId);
            entity.ToTable("DATA_PRODUCTS_INCLUDED");

            entity.Property(e => e.AllocationId).HasColumnName("ALLOCATION_ID");
            entity.Property(e => e.DataProductsIncludedId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DATA_PRODUCTS_INCLUDED_ID");
            entity.Property(e => e.Included)
                .HasDefaultValue(1)
                .HasColumnName("INCLUDED");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ITEM_ID");
            entity.Property(e => e.LevelBased)
                .HasDefaultValue(0)
                .HasColumnName("LEVEL_BASED");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(30)
                .HasColumnName("PRODUCT_CODE");
            entity.Property(e => e.ProductDesc)
                .HasMaxLength(500)
                .HasColumnName("PRODUCT_DESC");
            entity.Property(e => e.ProductLevel1)
                .HasMaxLength(20)
                .HasColumnName("PRODUCT_LEVEL_1");
            entity.Property(e => e.ProductLevel2)
                .HasMaxLength(20)
                .HasColumnName("PRODUCT_LEVEL_2");
            entity.Property(e => e.ProductLevel3)
                .HasMaxLength(20)
                .HasColumnName("PRODUCT_LEVEL_3");
            entity.Property(e => e.ProductType)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_TYPE");
        });

        modelBuilder.Entity<DataProductsUnknown>(entity =>
        {
            entity.HasKey(e => e.DataProductsUnknownId);
            entity.ToTable("DATA_PRODUCTS_UNKNOWN");

            entity.Property(e => e.DataProductsUnknownId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DATA_PRODUCTS_UNKNOWN_ID");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .HasColumnName("PRODUCT_CODE");
            entity.Property(e => e.ProductDesc)
                .HasMaxLength(700)
                .HasColumnName("PRODUCT_DESC");
            entity.Property(e => e.ProductLevel1)
                .HasMaxLength(50)
                .HasColumnName("PRODUCT_LEVEL_1");
            entity.Property(e => e.ProductLevel2)
                .HasMaxLength(50)
                .HasColumnName("PRODUCT_LEVEL_2");
            entity.Property(e => e.ProductLevel3)
                .HasMaxLength(50)
                .HasColumnName("PRODUCT_LEVEL_3");
        });

        //modelBuilder.Entity<DataPsoRate>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_PSO_RATES");

        //    entity.Property(e => e.CommissionRate)
        //        .HasColumnType("numeric(21, 7)")
        //        .HasColumnName("commissionRate");
        //    entity.Property(e => e.PayPlanType)
        //        .HasMaxLength(5)
        //        .HasColumnName("payPlanType");
        //    entity.Property(e => e.PeriodMonthEnd).HasColumnName("periodMonthEnd");
        //    entity.Property(e => e.PeriodMonthStart).HasColumnName("periodMonthStart");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(4)
        //        .HasColumnName("periodYear");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //});

        //modelBuilder.Entity<DataRevenueAgainstOrder>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_REVENUE_AGAINST_ORDER");

        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(50)
        //        .HasColumnName("employeeId");
        //    entity.Property(e => e.OrderId)
        //        .HasMaxLength(50)
        //        .HasColumnName("orderId");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasColumnName("periodMonth");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(5)
        //        .HasColumnName("periodYear");
        //    entity.Property(e => e.RevenueId).HasColumnName("revenueId");
        //    entity.Property(e => e.RevenueTotal)
        //        .HasColumnType("numeric(18, 5)")
        //        .HasColumnName("revenueTotal");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //    entity.Property(e => e.TimeStamp)
        //        .HasColumnType("datetime")
        //        .HasColumnName("timeStamp");
        //});

        //modelBuilder.Entity<DataRevenueTargetBucket>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_REVENUE_TARGET_BUCKET");

        //    entity.Property(e => e.AllocationId).HasColumnName("allocationId");
        //    entity.Property(e => e.LinkedAllocationId).HasColumnName("linkedAllocationId");
        //    entity.Property(e => e.LinkedAllocationThreshold).HasColumnName("linkedAllocationThreshold");
        //    entity.Property(e => e.Payplan)
        //        .HasMaxLength(5)
        //        .HasColumnName("payplan");
        //    entity.Property(e => e.PeriodMonthEnd).HasColumnName("periodMonthEnd");
        //    entity.Property(e => e.PeriodMonthStart).HasColumnName("periodMonthStart");
        //    entity.Property(e => e.PeriodYear).HasColumnName("periodYear");
        //    entity.Property(e => e.ProcessType)
        //        .HasMaxLength(5)
        //        .HasColumnName("processType");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //});

        //modelBuilder.Entity<DataRevenueUplift>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_REVENUE_UPLIFTS");

        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(50)
        //        .HasColumnName("employeeId");
        //    entity.Property(e => e.PeriodEnd).HasColumnName("periodEnd");
        //    entity.Property(e => e.PeriodStart).HasColumnName("periodStart");
        //    entity.Property(e => e.PeriodYear).HasColumnName("periodYear");
        //    entity.Property(e => e.RepType)
        //        .HasMaxLength(5)
        //        .HasColumnName("repType");
        //    entity.Property(e => e.RevenueId).HasColumnName("revenueId");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //    entity.Property(e => e.UpliftPercentage)
        //        .HasColumnType("numeric(21, 7)")
        //        .HasColumnName("upliftPercentage");
        //});

        //modelBuilder.Entity<DataUnitsOverride>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("DATA_UNITS_OVERRIDE");

        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(50)
        //        .HasColumnName("employeeId");
        //    entity.Property(e => e.Override).HasColumnName("override");
        //    entity.Property(e => e.PayPlanType)
        //        .HasMaxLength(5)
        //        .HasColumnName("payPlanType");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(5)
        //        .HasColumnName("periodYear");
        //    entity.Property(e => e.PositionId)
        //        .HasMaxLength(50)
        //        .HasColumnName("positionId");
        //    entity.Property(e => e.RowId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("rowId");
        //});

        //modelBuilder.Entity<DeductionExclusionRep>(entity =>
        //{
        //    entity.HasKey(e => e.DeductionExclusionRepsId)
        //        .HasName("PK_DEDUCTION_EXCLUSION_REPS_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("DEDUCTION_EXCLUSION_REPS");

        //    entity.Property(e => e.DeductionExclusionRepsId).HasColumnName("DEDUCTION_EXCLUSION_REPS_ID");
        //    entity.Property(e => e.Authorised)
        //        .HasMaxLength(100)
        //        .HasColumnName("AUTHORISED");
        //    entity.Property(e => e.DateStamp)
        //        .HasComputedColumnSql("(getdate())", false)
        //        .HasColumnType("datetime")
        //        .HasColumnName("DATE_STAMP");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasComputedColumnSql("([dbo].[ufn_getEmployeeID]([ROW_ID]))", false)
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.EnteredBy)
        //        .HasMaxLength(128)
        //        .HasComputedColumnSql("(suser_sname())", false)
        //        .HasColumnName("ENTERED_BY");
        //    entity.Property(e => e.ExclusionReason).HasColumnName("EXCLUSION_REASON");
        //    entity.Property(e => e.PayPlanType)
        //        .HasMaxLength(5)
        //        .HasComputedColumnSql("([dbo].[ufn_getPayPlan]([ROW_ID]))", false)
        //        .HasColumnName("PAY_PLAN_TYPE");
        //    entity.Property(e => e.RowId)
        //        .HasMaxLength(30)
        //        .HasComment("This is effectively the ROW_ID from the TMP_DATA_POSITIONS table")
        //        .HasColumnName("ROW_ID");
        //    entity.Property(e => e.Status)
        //        .HasDefaultValue(1)
        //        .HasColumnName("STATUS");
        //});

        //modelBuilder.Entity<FpsmAllocation>(entity =>
        //{
        //    entity.HasKey(e => e.FpsmAllocationId)
        //        .HasName("PK_FPSM_ALLOCATION_ID")
        //        .HasFillFactor(90);

        //    entity.ToTable("FPSM_ALLOCATION");

        //    entity.Property(e => e.FpsmAllocationId).HasColumnName("FPSM_ALLOCATION_ID");
        //    entity.Property(e => e.AdjustmentId).HasColumnName("ADJUSTMENT_ID");
        //    entity.Property(e => e.AllocationList)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("ALLOCATION_LIST");
        //    entity.Property(e => e.AllocationSource)
        //        .HasMaxLength(30)
        //        .HasDefaultValue("PRODUCT_ITEM")
        //        .HasColumnName("ALLOCATION_SOURCE");
        //    entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
        //    entity.Property(e => e.AllocationValue)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("ALLOCATION_VALUE");
        //    entity.Property(e => e.ChildPositionId)
        //        .HasMaxLength(30)
        //        .HasColumnName("CHILD_POSITION_ID");
        //    entity.Property(e => e.DateCreated)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("DATE_CREATED");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.ExcludeFromCalcs)
        //        .HasMaxLength(1)
        //        .HasColumnName("EXCLUDE_FROM_CALCS");
        //    entity.Property(e => e.ItemId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ITEM_ID");
        //    entity.Property(e => e.OrderId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ORDER_ID");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_MONTH'))")
        //        .HasColumnName("PERIOD_MONTH");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("PERIOD_YEAR");
        //    entity.Property(e => e.PositionId)
        //        .HasMaxLength(30)
        //        .HasColumnName("POSITION_ID");
        //    entity.Property(e => e.RollupProcessed)
        //        .HasMaxLength(1)
        //        .HasColumnName("ROLLUP_PROCESSED");
        //});

        //modelBuilder.Entity<FpsmTempAllocation>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("FPSM_TEMP_ALLOCATIONS");

        //    entity.Property(e => e.AdjustmentId).HasColumnName("ADJUSTMENT_ID");
        //    entity.Property(e => e.AllocationList)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("ALLOCATION_LIST");
        //    entity.Property(e => e.AllocationSource)
        //        .HasMaxLength(30)
        //        .HasColumnName("ALLOCATION_SOURCE");
        //    entity.Property(e => e.AllocationTypeId).HasColumnName("ALLOCATION_TYPE_ID");
        //    entity.Property(e => e.AllocationValue)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("ALLOCATION_VALUE");
        //    entity.Property(e => e.ChildPositionId)
        //        .HasMaxLength(30)
        //        .HasColumnName("CHILD_POSITION_ID");
        //    entity.Property(e => e.DateCreated)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime")
        //        .HasColumnName("DATE_CREATED");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.ExcludeFromCalcs)
        //        .HasMaxLength(1)
        //        .HasColumnName("EXCLUDE_FROM_CALCS");
        //    entity.Property(e => e.FpsmAllocationId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnName("FPSM_ALLOCATION_ID");
        //    entity.Property(e => e.ItemId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ITEM_ID");
        //    entity.Property(e => e.OrderId)
        //        .HasMaxLength(30)
        //        .HasColumnName("ORDER_ID");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasColumnName("PERIOD_MONTH");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(4)
        //        .HasColumnName("PERIOD_YEAR");
        //    entity.Property(e => e.PositionId)
        //        .HasMaxLength(30)
        //        .HasColumnName("POSITION_ID");
        //    entity.Property(e => e.RollupProcessed)
        //        .HasMaxLength(1)
        //        .HasColumnName("ROLLUP_PROCESSED");
        //});

        modelBuilder.Entity<OrdersToProcess>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OrdersToProcess");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .HasColumnName("order_id");
            entity.Property(e => e.StatusDt)
                .HasColumnType("datetime")
                .HasColumnName("status_dt");
        });

        modelBuilder.Entity<PayplanUnset>(entity =>
        {
            entity.HasKey(e => e.PayplanUnsetId)
                 .HasName("PK_FPSM_ALLOCATION_ID");

            entity.ToTable("PAYPLAN_UNSET");

            entity.Property(e => e.Orderid)
                .HasMaxLength(30)
                .HasColumnName("ORDERID");
            entity.Property(e => e.Payplan)
                .HasMaxLength(10)
                .HasColumnName("PAYPLAN");
            entity.Property(e => e.PayplanUnsetId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PAYPLAN_UNSET_ID");
        });

        modelBuilder.Entity<RunIcmInfo>(entity =>
        {
            entity.HasKey(e => e.RunIcmInfoId);
            entity.ToTable("RUN_ICM_INFO");

            entity.Property(e => e.BeginDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("BEGIN_DATE");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("END_DATE");
            entity.Property(e => e.RunIcmInfoId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RUN_ICM_INFO_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasColumnName("STATUS");
        });

        //modelBuilder.Entity<SupplyAdjustment>(entity =>
        //{
        //    entity.HasKey(e => e.SupplyAdjustmentsId).HasFillFactor(90);

        //    entity.ToTable("SUPPLY_ADJUSTMENTS");

        //    entity.Property(e => e.SupplyAdjustmentsId).HasColumnName("SUPPLY_ADJUSTMENTS_ID");
        //    entity.Property(e => e.AdjustmentAmount)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("ADJUSTMENT_AMOUNT");
        //    entity.Property(e => e.EmployeeId)
        //        .HasMaxLength(30)
        //        .HasColumnName("EMPLOYEE_ID");
        //    entity.Property(e => e.EmployeeName)
        //        .HasMaxLength(101)
        //        .HasColumnName("EMPLOYEE_NAME");
        //    entity.Property(e => e.EmployeeNumber)
        //        .HasMaxLength(100)
        //        .HasColumnName("EMPLOYEE_NUMBER");
        //    entity.Property(e => e.Manager)
        //        .HasMaxLength(100)
        //        .HasDefaultValue("Sam Patel")
        //        .HasColumnName("MANAGER");
        //    entity.Property(e => e.PeriodMonth)
        //        .HasMaxLength(2)
        //        .HasDefaultValue("00")
        //        .HasColumnName("PERIOD_MONTH");
        //    entity.Property(e => e.PeriodYear)
        //        .HasMaxLength(4)
        //        .HasDefaultValueSql("([dbo].[ufn_getSysParm]('PERIOD_YEAR'))")
        //        .HasColumnName("PERIOD_YEAR");
        //    entity.Property(e => e.PositionId)
        //        .HasMaxLength(30)
        //        .HasColumnName("POSITION_ID");
        //});

        modelBuilder.Entity<SystemLog>(entity =>
        {
            entity.HasKey(e => e.SystemLogId)
                .HasName("PK_SYSTEM_LOG_ID")
                .HasFillFactor(90);

            entity.ToTable("SYSTEM_LOG");

            entity.Property(e => e.SystemLogId).HasColumnName("SYSTEM_LOG_ID");
            entity.Property(e => e.LogCategory)
                .HasMaxLength(100)
                .HasColumnName("LOG_CATEGORY");
            entity.Property(e => e.LogCountId).HasColumnName("LOG_COUNT_ID");
            entity.Property(e => e.LogDatetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("LOG_DATETIME");
            entity.Property(e => e.LogDesc).HasColumnName("LOG_DESC");
            entity.Property(e => e.LogSource)
                .HasMaxLength(4000)
                .HasColumnName("LOG_SOURCE");
            entity.Property(e => e.ReferenceType)
                .HasMaxLength(4000)
                .HasColumnName("REFERENCE_TYPE");
            entity.Property(e => e.ReferenceValue)
                .HasMaxLength(4000)
                .HasColumnName("REFERENCE_VALUE");
        });

        modelBuilder.Entity<TmpDataEmployee>(entity =>
        {
            entity.HasKey(e => e.TmpDataEmployeesId)
                .HasName("PK_TMP_DATA_EMPLOYEES_ID")
                .HasFillFactor(90);

            entity.ToTable("TMP_DATA_EMPLOYEES");

            entity.Property(e => e.TmpDataEmployeesId).HasColumnName("TMP_DATA_EMPLOYEES_ID");
            entity.Property(e => e.EmailAddr)
                .HasMaxLength(200)
                .HasColumnName("EMAIL_ADDR");
            entity.Property(e => e.EmployeeNumber)
                .HasMaxLength(100)
                .HasColumnName("EMPLOYEE_NUMBER");
            entity.Property(e => e.FstName)
                .HasMaxLength(100)
                .HasColumnName("FST_NAME");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(150)
                .HasColumnName("JOB_TITLE");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("LOGIN");
            entity.Property(e => e.PrHeldPostnId)
                .HasMaxLength(30)
                .HasColumnName("PR_HELD_POSTN_ID");
            entity.Property(e => e.RowId)
                .HasMaxLength(30)
                .HasColumnName("ROW_ID");
        });

        modelBuilder.Entity<TmpDataImpressSubscriptionOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TMP_DATA_IMPRESS_SUBSCRIPTION_ORDER");

            entity.Property(e => e.DataImpressSubscriptionOrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DATA_IMPRESS_SUBSCRIPTION_ORDER_ID");
            entity.Property(e => e.ImAccountName)
                .HasMaxLength(50)
                .HasColumnName("IM_ACCOUNT_NAME");
            entity.Property(e => e.ImAccountNumber)
                .HasMaxLength(50)
                .HasColumnName("IM_ACCOUNT_NUMBER");
            entity.Property(e => e.ImActualUsage).HasColumnName("IM_ACTUAL_USAGE");
            entity.Property(e => e.ImOrderNumber)
                .HasMaxLength(50)
                .HasColumnName("IM_ORDER_NUMBER");
            entity.Property(e => e.ImPlanName)
                .HasMaxLength(50)
                .HasColumnName("IM_PLAN_NAME");
            entity.Property(e => e.ImPlanTier)
                .HasMaxLength(50)
                .HasColumnName("IM_PLAN_TIER");
            entity.Property(e => e.ImPricePerUsageItem)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("IM_PRICE_PER_USAGE_ITEM");
            entity.Property(e => e.SfAnticipatedUsage)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("SF_ANTICIPATED_USAGE");
            entity.Property(e => e.SfEmployeeId)
                .HasMaxLength(50)
                .HasColumnName("SF_EMPLOYEE_ID");
            entity.Property(e => e.SfLastStatusUpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("SF_LAST_STATUS_UPDATE_TIME");
            entity.Property(e => e.SfOrderNumber)
                .HasMaxLength(50)
                .HasColumnName("SF_ORDER_NUMBER");
            entity.Property(e => e.SfOrderStatus)
                .HasMaxLength(150)
                .HasColumnName("SF_ORDER_STATUS");
            entity.Property(e => e.SfSubscriptionMonthsAmount).HasColumnName("SF_SUBSCRIPTION_MONTHS_AMOUNT");
            entity.Property(e => e.SfSubscriptionPrice)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("SF_SUBSCRIPTION_PRICE");
        });

        modelBuilder.Entity<TmpDataOrderHeader>(entity =>
        {
            entity.HasKey(e => e.TmpDataOrderHeaderId).HasFillFactor(90);

            entity.ToTable("TMP_DATA_ORDER_HEADER");

            entity.HasIndex(e => new { e.OrderType, e.RowId }, "IX_TMP_DATA_ORDER_HEADER_ORDER_TYPE").HasFillFactor(90);

            entity.HasIndex(e => e.RowId, "IX_TMP_DATA_ORDER_HEADER_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.TmpDataOrderHeaderId).HasColumnName("TMP_DATA_ORDER_HEADER_ID");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(60)
                .HasColumnName("ACCOUNT_NUMBER");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .HasColumnName("CUSTOMER_NAME");
            entity.Property(e => e.CustomerType)
                .HasMaxLength(60)
                .HasColumnName("CUSTOMER_TYPE");
            entity.Property(e => e.MaintenanceTerm)
                .HasMaxLength(50)
                .HasColumnName("MAINTENANCE_TERM");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(60)
                .HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.OrderType)
                .HasMaxLength(100)
                .HasColumnName("ORDER_TYPE");
            entity.Property(e => e.PrPostnId)
                .HasMaxLength(30)
                .HasColumnName("PR_POSTN_ID");
            entity.Property(e => e.PromotionCode)
                .HasMaxLength(60)
                .HasColumnName("PROMOTION_CODE");
            entity.Property(e => e.RowId)
                .HasMaxLength(30)
                .HasColumnName("ROW_ID");
            entity.Property(e => e.SapReference)
                .HasMaxLength(20)
                .HasColumnName("SAP_REFERENCE");
        });

        modelBuilder.Entity<TmpDataOrderItem>(entity =>
        {
            entity.HasKey(e => e.TmpDataOrderItemId)
                .HasName("PK_TMP_DATA_ORDER_ITEM_ID")
                .HasFillFactor(90);

            entity.ToTable("TMP_DATA_ORDER_ITEM");

            entity.HasIndex(e => e.OrderRowId, "IX_TMP_DATA_ORDER_ITEM_ORDER_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.TmpDataOrderItemId).HasColumnName("TMP_DATA_ORDER_ITEM_ID");
            entity.Property(e => e.LineNumber).HasColumnName("LINE_NUMBER");
            entity.Property(e => e.ListValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("LIST_VALUE");
            entity.Property(e => e.OrderItemId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ITEM_ID");
            entity.Property(e => e.OrderRowId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_ROW_ID");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(200)
                .HasColumnName("PRODUCT_CODE");
            entity.Property(e => e.ProductDesc)
                .HasMaxLength(510)
                .HasColumnName("PRODUCT_DESC");
            entity.Property(e => e.ProductLevel1)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_1");
            entity.Property(e => e.ProductLevel2)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_2");
            entity.Property(e => e.ProductLevel3)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_LEVEL_3");
            entity.Property(e => e.ProductType)
                .HasMaxLength(80)
                .HasColumnName("PRODUCT_TYPE");
            entity.Property(e => e.Quantity)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.SaleValue)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("SALE_VALUE");
        });

        modelBuilder.Entity<TmpDataOrderRep>(entity =>
        {
            entity.HasKey(e => e.TmpDataOrderRepsId)
                .HasName("PK_TMP_DATA_ORDER_REPS_ID")
                .HasFillFactor(90);

            entity.ToTable("TMP_DATA_ORDER_REPS");

            entity.HasIndex(e => e.OrderPosId, "idx_TMP_DATA_ORDER_REPS_ORDER_POS_ID").HasFillFactor(90);

            entity.HasIndex(e => e.RowId, "idx_TMP_DATA_ORDER_REPS_ROW_ID").HasFillFactor(90);

            entity.Property(e => e.TmpDataOrderRepsId).HasColumnName("TMP_DATA_ORDER_REPS_ID");
            entity.Property(e => e.AllocationPerc)
                .HasColumnType("numeric(22, 7)")
                .HasColumnName("ALLOCATION_PERC");
            entity.Property(e => e.OrderPosId)
                .HasMaxLength(30)
                .HasColumnName("ORDER_POS_ID");
            entity.Property(e => e.PositionId)
                .HasMaxLength(30)
                .HasColumnName("POSITION_ID");
            entity.Property(e => e.RowId)
                .HasMaxLength(30)
                .HasColumnName("ROW_ID");
        });

        modelBuilder.Entity<TmpDataOrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.TmpDataOrderStatusHistoryId)
                .HasName("PK_TMP_DATA_ORDER_STATUS_HISTORY_ID")
                .HasFillFactor(90);

            entity.ToTable("TMP_DATA_ORDER_STATUS_HISTORY");

            entity.Property(e => e.TmpDataOrderStatusHistoryId).HasColumnName("TMP_DATA_ORDER_STATUS_HISTORY_ID");
            entity.Property(e => e.BuName)
                .HasMaxLength(60)
                .HasColumnName("buName");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.RowId)
                .HasMaxLength(15)
                .HasColumnName("rowId");
        });

        modelBuilder.Entity<TmpDataPosition>(entity =>
        {
            entity.HasKey(e => e.TmpDataPositionsId)
                .HasName("PK_TMP_DATA_POSITIONS_ID")
                .HasFillFactor(90);

            entity.ToTable("TMP_DATA_POSITIONS");

            entity.HasIndex(e => e.RowId, "missing_index_1113").HasFillFactor(90);

            entity.Property(e => e.TmpDataPositionsId).HasColumnName("TMP_DATA_POSITIONS_ID");
            entity.Property(e => e.EndDt)
                .HasColumnType("datetime")
                .HasColumnName("END_DT");
            entity.Property(e => e.ParentRowId)
                .HasMaxLength(30)
                .HasColumnName("PARENT_ROW_ID");
            entity.Property(e => e.PositionName)
                .HasMaxLength(100)
                .HasColumnName("POSITION_NAME");
            entity.Property(e => e.PrEmpId)
                .HasMaxLength(30)
                .HasColumnName("PR_EMP_ID");
            entity.Property(e => e.RowId)
                .HasMaxLength(30)
                .HasColumnName("ROW_ID");
            entity.Property(e => e.StartDt)
                .HasColumnType("datetime")
                .HasColumnName("START_DT");
            entity.Property(e => e.XRepType)
                .HasMaxLength(60)
                .HasColumnName("X_REP_TYPE");
        });

        //modelBuilder.Entity<VTblTargetReport>(entity =>
        //{
        //    entity.HasKey(e => e.Id)
        //        .HasName("PK__v_tblTargetRepor__3E730AF3")
        //        .HasFillFactor(90);

        //    entity.ToTable("v_tblTargetReport");

        //    entity.Property(e => e.Id).HasColumnName("id");
        //    entity.Property(e => e.EmployeeName)
        //        .HasMaxLength(200)
        //        .IsUnicode(false)
        //        .HasColumnName("employee_name");
        //    entity.Property(e => e.EmployeeRowId)
        //        .HasMaxLength(50)
        //        .IsUnicode(false)
        //        .HasColumnName("employee_row_id");
        //    entity.Property(e => e.RevenueTotal)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("revenue_total");
        //    entity.Property(e => e.TargetTotal)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("target_total");
        //});

        //modelBuilder.Entity<VTblTargetReportS6>(entity =>
        //{
        //    entity.HasKey(e => e.Id)
        //        .HasName("PK__v_tblTargetRepor__2D1E61BF")
        //        .HasFillFactor(90);

        //    entity.ToTable("v_tblTargetReport_s6");

        //    entity.Property(e => e.Id).HasColumnName("id");
        //    entity.Property(e => e.EmployeeName)
        //        .HasMaxLength(200)
        //        .IsUnicode(false)
        //        .HasColumnName("employee_name");
        //    entity.Property(e => e.EmployeeRowId)
        //        .HasMaxLength(50)
        //        .IsUnicode(false)
        //        .HasColumnName("employee_row_id");
        //    entity.Property(e => e.RevenueTotal)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("revenue_total");
        //    entity.Property(e => e.TargetTotal)
        //        .HasColumnType("numeric(22, 7)")
        //        .HasColumnName("target_total");
        //});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
