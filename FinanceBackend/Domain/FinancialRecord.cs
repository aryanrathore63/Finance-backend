namespace FinanceBackend.Domain;

public enum TransactionType
{
    Income,
    Expense
}

public class FinancialRecord
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Track who created the record
    public int CreatedByUserId { get; set; }
}
