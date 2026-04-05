using FinanceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public DashboardController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [HttpGet("summary")]
    [Authorize(Policy = "ViewerPlus")] // Everyone (Viewer, Analyst, Admin) can view summary
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _financeService.GetDashboardSummaryAsync();
        return Ok(summary);
    }

    [HttpGet("insights")]
    [Authorize(Policy = "AnalystPlus")] // Only Analyst and Admin can view insights
    public async Task<IActionResult> GetInsights()
    {
        var insights = await _financeService.GetInsightsAsync();
        return Ok(insights);
    }
}
