using Microsoft.AspNetCore.Mvc;
using QuestionarioApp.Models;
using QuestionarioApp.Services;

namespace QuestionarioApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _service;

    public QuestionsController(IQuestionService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Question>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Question> GetById(Guid id)
    {
        var item = _service.GetById(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpGet("by-questionnaire/{questionnaireId:guid}")]
    public ActionResult<IEnumerable<Question>> GetByQuestionnaire(Guid questionnaireId)
    {
        return Ok(_service.GetByQuestionnaire(questionnaireId));
    }

    [HttpPost]
    public ActionResult<Question> Create([FromBody] Question input)
    {
        var created = _service.Create(input);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] Question input)
    {
        var ok = _service.Update(id, input);
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
