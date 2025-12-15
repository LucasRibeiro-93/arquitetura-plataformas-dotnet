using Microsoft.AspNetCore.Mvc;
using QuestionarioApp.Models;
using QuestionarioApp.Services;

namespace QuestionarioApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantService _service;

    public ParticipantsController(IParticipantService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Participant>> GetAll() => Ok(_service.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<Participant> GetById(Guid id)
    {
        var item = _service.GetById(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public ActionResult<Participant> Create([FromBody] Participant input)
    {
        var created = _service.Create(input);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] Participant input)
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
