using Microsoft.AspNetCore.Mvc;
using QuestionarioApp.Services;

namespace QuestionarioApp.Controllers;

[ApiController]
[Route("api/admin/reports")] // admin area (no auth yet)
public class AdminReportsController : ControllerBase
{
    private readonly IReportingService _reporting;

    public AdminReportsController(IReportingService reporting)
    {
        _reporting = reporting;
    }

    /// <summary>
    /// Returns a summarized view for a single questionnaire.
    /// </summary>
    [HttpGet("questionnaires/{id:guid}/summary")]
    public ActionResult<QuestionnaireSummaryDto> GetQuestionnaireSummary(Guid id)
    {
        var summary = _reporting.GetQuestionnaireSummary(id);
        if (summary is null) return NotFound();
        return Ok(summary);
    }

    /// <summary>
    /// Returns summaries for all questionnaires (each summary is per-questionnaire; no cross-aggregation).
    /// </summary>
    [HttpGet("questionnaires/summary")]
    public ActionResult<IEnumerable<QuestionnaireSummaryDto>> GetAllQuestionnairesSummaries()
    {
        var list = _reporting.GetAllQuestionnairesSummaries();
        return Ok(list);
    }
}
