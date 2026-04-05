using FinanceBackend.Data;
using FinanceBackend.DTOs;
using FinanceBackend.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinanceBackend.Services;

public interface IFinanceService
{
    Task<IEnumerable<FinancialRecordResponseDto>> GetRecordsAsync(string? category, TransactionType? type, DateTime? startDate, DateTime? endDate);
    Task<FinancialRecordResponseDto?> GetRecordByIdAsync(int id);
    Task<FinancialRecordResponseDto> CreateRecordAsync(FinancialRecordCreateDto recordDto, int userId);
    Task<bool> UpdateRecordAsync(int id, FinancialRecordUpdateDto recordDto);
    Task<bool> DeleteRecordAsync(int id);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    Task<InsightSummaryDto> GetInsightsAsync();
}

public class FinanceService : IFinanceService
{
    private readonly AppDbContext _context;

    public FinanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FinancialRecordResponseDto>> GetRecordsAsync(string? category, TransactionType? type, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.FinancialRecords.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(r => r.Category == category);
        
        if (type.HasValue)
            query = query.Where(r => r.Type == type.Value);
        
        if (startDate.HasValue)
            query = query.Where(r => r.Date >= startDate.Value);
        
        if (endDate.HasValue)
            query = query.Where(r => r.Date <= endDate.Value);

        var records = await query.ToListAsync();
        return records.Select(r => MapToResponse(r));
    }

    public async Task<FinancialRecordResponseDto?> GetRecordByIdAsync(int id)
    {
        var record = await _context.FinancialRecords.FindAsync(id);
        return record == null ? null : MapToResponse(record);
    }

    public async Task<FinancialRecordResponseDto> CreateRecordAsync(FinancialRecordCreateDto recordDto, int userId)
    {
        var record = new FinancialRecord
        {
            Amount = recordDto.Amount,
            Type = recordDto.Type,
            Category = recordDto.Category,
            Date = recordDto.Date,
            Notes = recordDto.Notes,
            CreatedByUserId = userId
        };

        _context.FinancialRecords.Add(record);
        await _context.SaveChangesAsync();
        return MapToResponse(record);
    }

    public async Task<bool> UpdateRecordAsync(int id, FinancialRecordUpdateDto recordDto)
    {
        var record = await _context.FinancialRecords.FindAsync(id);
        if (record == null) return false;

        record.Amount = recordDto.Amount;
        record.Type = recordDto.Type;
        record.Category = recordDto.Category;
        record.Date = recordDto.Date;
        record.Notes = recordDto.Notes;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRecordAsync(int id)
    {
        var record = await _context.FinancialRecords.FindAsync(id);
        if (record == null) return false;

        _context.FinancialRecords.Remove(record);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var records = await _context.FinancialRecords.ToListAsync();
        
        var totalIncome = records.Where(r => r.Type == TransactionType.Income).Sum(r => r.Amount);
        var totalExpenses = records.Where(r => r.Type == TransactionType.Expense).Sum(r => r.Amount);
        
        var categoryBreakdown = records
            .GroupBy(r => r.Category)
            .ToDictionary(g => g.Key, g => g.Sum(r => r.Amount));

        return new DashboardSummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            NetBalance = totalIncome - totalExpenses,
            CategoryBreakdown = categoryBreakdown
        };
    }

    public async Task<InsightSummaryDto> GetInsightsAsync()
    {
        var summary = await GetDashboardSummaryAsync();
        var recentRecords = await _context.FinancialRecords
            .OrderByDescending(r => r.Date)
            .Take(5)
            .ToListAsync();

        var monthlyTrends = await _context.FinancialRecords
            .GroupBy(r => new { r.Date.Year, r.Date.Month })
            .Select(g => new TrendPointDto
            {
                Label = $"{g.Key.Year}-{g.Key.Month:D2}",
                Value = g.Sum(r => r.Type == TransactionType.Income ? r.Amount : -r.Amount)
            })
            .OrderBy(t => t.Label)
            .ToListAsync();

        return new InsightSummaryDto
        {
            TotalIncome = summary.TotalIncome,
            TotalExpenses = summary.TotalExpenses,
            NetBalance = summary.NetBalance,
            CategoryBreakdown = summary.CategoryBreakdown,
            RecentActivity = recentRecords.Select(r => new RecentActivityDto
            {
                Id = r.Id,
                Description = r.Notes,
                Amount = r.Amount,
                Type = r.Type.ToString(),
                Date = r.Date.ToShortDateString()
            }).ToList(),
            MonthlyTrends = monthlyTrends
        };
    }

    private static FinancialRecordResponseDto MapToResponse(FinancialRecord record)
    {
        return new FinancialRecordResponseDto
        {
            Id = record.Id,
            Amount = record.Amount,
            Type = record.Type,
            Category = record.Category,
            Date = record.Date,
            Notes = record.Notes,
            CreatedAt = record.CreatedAt
        };
    }
}
