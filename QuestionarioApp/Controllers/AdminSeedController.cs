using Microsoft.AspNetCore.Mvc;
using QuestionarioApp.Services;

namespace QuestionarioApp.Controllers;

[ApiController]
[Route("api/admin/seed")] // admin area for demo data
public class AdminSeedController : ControllerBase
{
    private readonly IDemoSeeder _seeder;

    public AdminSeedController(IDemoSeeder seeder)
    {
        _seeder = seeder;
    }

    /// <summary>
    /// Populates the in-memory store with demo data for showcasing reports.
    /// </summary>
    /// <param name="reset">If true, clears existing in-memory data first (default: true).</param>
    /// <param name="seed">Optional seed for deterministic generation.</param>
    /// <param name="questionnaires">Number of questionnaires (default: 3)</param>
    /// <param name="questionsPerQuestionnaire">Questions per questionnaire (default: 6)</param>
    /// <param name="answersPerQuestion">Answers per question (default: 4)</param>
    /// <param name="participants">Number of participants (default: 20)</param>
    /// <param name="assignments">Number of assignments to create (default: 60)</param>
    /// <param name="completionRate">Fraction 0..1 of assignments to mark as completed (default: 0.7)</param>
    [HttpPost("demo")]
    public ActionResult<DemoSeedResult> SeedDemo(
        [FromQuery] bool reset = true,
        [FromQuery] int? seed = null,
        [FromQuery] int questionnaires = 3,
        [FromQuery] int questionsPerQuestionnaire = 6,
        [FromQuery] int answersPerQuestion = 4,
        [FromQuery] int participants = 20,
        [FromQuery] int assignments = 60,
        [FromQuery] double completionRate = 0.7)
    {
        if (completionRate < 0) completionRate = 0;
        if (completionRate > 1) completionRate = 1;

        var result = _seeder.SeedDemo(reset, seed, questionnaires, questionsPerQuestionnaire, answersPerQuestion,
            participants, assignments, completionRate);
        return Ok(result);
    }
}
