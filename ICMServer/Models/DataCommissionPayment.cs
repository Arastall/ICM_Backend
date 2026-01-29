using System.ComponentModel.DataAnnotations.Schema;

[Table("DATA_COMMISSION_PAYMENT")]
public partial class DataCommissionPayment
{
    [Column("DATA_COMMISSION_PAYMENT_ID")]
    public int DataCommissionPaymentId { get; set; }

    [Column("POSITION_ID")]
    public string? PositionId { get; set; }

    [Column("EMPLOYEE_ID")]
    public string? EmployeeId { get; set; }

    [Column("PAYMENT_SOURCE")]
    public string? PaymentSource { get; set; }

    [Column("PAYMENT_DESCRIPTION")]
    public string? PaymentDescription { get; set; }

    [Column("ORDER_ID")]
    public string? OrderId { get; set; }

    [Column("ENHANCEMENT_ID")]
    public int? EnhancementId { get; set; }

    [Column("PAYPLAN_RATE_ID")]
    public int? PayplanRateId { get; set; }

    [Column("ADJUSTMENT_ID")]
    public int? AdjustmentId { get; set; }

    [Column("PAYMENT_RATE")]
    public decimal? PaymentRate { get; set; }

    [Column("PAYMENT_VALUE")]
    public decimal? PaymentValue { get; set; }

    [Column("PAYMENT_WITHELD")]
    public bool PaymentWitheld { get; set; }

    [Column("PERIOD_MONTH")]
    public string PeriodMonth { get; set; } = null!;

    [Column("PERIOD_YEAR")]
    public string PeriodYear { get; set; } = null!;

    [Column("DATE_CREATED")]
    public DateTime DateCreated { get; set; }

    /*[Column("CREDIT_ALLOCATION_ID")]
    public int? CreditAllocationId { get; set; }*/
}
