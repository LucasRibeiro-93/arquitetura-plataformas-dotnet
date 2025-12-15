using Microsoft.AspNetCore.Mvc;
using QuestionarioApp.Models;
using QuestionarioApp.Services;

namespace QuestionarioApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionnairesController : ControllerBase
{
    private readonly IQuestionnaireService _service;

    public QuestionnairesController(IQuestionnaireService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Questionnaire>> GetAll()
    {
        var list = _service.GetAll();
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Questionnaire> GetById(Guid id)
    {
        var item = _service.GetById(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public ActionResult<Questionnaire> Create([FromBody] Questionnaire input)
    {
        var created = _service.Create(input);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] Questionnaire input)
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
