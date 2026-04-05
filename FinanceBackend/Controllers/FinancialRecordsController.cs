using System.Security.Claims;
using FinanceBackend.DTOs;
using FinanceBackend.Domain;
using FinanceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // All endpoints require authentication
public class FinancialRecordsController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public FinancialRecordsController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [HttpGet]
    [Authorize(Policy = "AnalystPlus")] // Analyst and Admin can view list
    public async Task<IActionResult> GetRecords(
        [FromQuery] string? category, 
        [FromQuery] TransactionType? type, 
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate)
    {
        var records = await _financeService.GetRecordsAsync(category, type, startDate, endDate);
        return Ok(records);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "AnalystPlus")]
    public async Task<IActionResult> GetRecord(int id)
    {
        var record = await _financeService.GetRecordByIdAsync(id);
        if (record == null) return NotFound();
        return Ok(record);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")] // Only Admin can create
    public async Task<IActionResult> CreateRecord(FinancialRecordCreateDto recordDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _financeService.CreateRecordAsync(recordDto, userId);
        return CreatedAtAction(nameof(GetRecord), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")] // Only Admin can update
    public async Task<IActionResult> UpdateRecord(int id, FinancialRecordUpdateDto recordDto)
    {
        var result = await _financeService.UpdateRecordAsync(id, recordDto);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")] // Only Admin can delete
    public async Task<IActionResult> DeleteRecord(int id)
    {
        var result = await _financeService.DeleteRecordAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
