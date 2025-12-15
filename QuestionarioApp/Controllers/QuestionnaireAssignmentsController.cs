using Microsoft.AspNetCore.Mvc;
using QuestionarioApp.Models;
using QuestionarioApp.Services;

namespace QuestionarioApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionnaireAssignmentsController : ControllerBase
{
    private readonly IQuestionnaireAssignmentService _service;

    public QuestionnaireAssignmentsController(IQuestionnaireAssignmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<QuestionnaireAssignment>> GetAll() => Ok(_service.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<QuestionnaireAssignment> GetById(Guid id)
    {
        var item = _service.GetById(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpGet("by-participant/{participantId:guid}")]
    public ActionResult<IEnumerable<QuestionnaireAssignment>> GetByParticipant(Guid participantId)
        => Ok(_service.GetByParticipant(participantId));

    [HttpGet("by-questionnaire/{questionnaireId:guid}")]
    public ActionResult<IEnumerable<QuestionnaireAssignment>> GetByQuestionnaire(Guid questionnaireId)
        => Ok(_service.GetByQuestionnaire(questionnaireId));

    [HttpPost]
    public ActionResult<QuestionnaireAssignment> Create([FromBody] QuestionnaireAssignment input)
    {
        var created = _service.Create(input);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] QuestionnaireAssignment input)
    {
        var ok = _service.Update(id, input);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpPost("{id:guid}/submit")]
    public IActionResult Submit(Guid id, [FromBody] IEnumerable<AssignmentResponse> responses)
    {
        var ok = _service.Submit(id, responses);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var ok = _service.Delete(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
