namespace FinanceBackend.DTOs;

public class DashboardSummaryDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetBalance { get; set; }
    public Dictionary<string, decimal> CategoryBreakdown { get; set; } = [];
}

public class RecentActivityDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

public class TrendPointDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public class InsightSummaryDto : DashboardSummaryDto
{
    public List<RecentActivityDto> RecentActivity { get; set; } = [];
    public List<TrendPointDto> MonthlyTrends { get; set; } = [];
}
