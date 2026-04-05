using FinanceBackend.Domain;

namespace FinanceBackend.DTOs;

public class FinancialRecordCreateDto
{
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class FinancialRecordUpdateDto : FinancialRecordCreateDto
{
    // If there were any differences, they'd go here.
}

public class FinancialRecordResponseDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
