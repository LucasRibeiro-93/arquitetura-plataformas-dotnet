using QuestionarioApp.Models;
using QuestionarioApp.Repositories;

namespace QuestionarioApp.Services;

public class QuestionnaireService : IQuestionnaireService
{
    private readonly IQuestionnaireRepository _repo;

    public QuestionnaireService(IQuestionnaireRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Questionnaire> GetAll() => _repo.GetAll();

    public Questionnaire? GetById(Guid id) => _repo.Get(id);

    public Questionnaire Create(Questionnaire input)
    {
        if (string.IsNullOrWhiteSpace(input.Title))
            throw new ArgumentException("Title is required", nameof(input.Title));

        var now = DateTime.UtcNow;
        var entity = new Questionnaire
        {
            Id = input.Id == Guid.Empty ? Guid.NewGuid() : input.Id,
            Title = input.Title.Trim(),
            Description = input.Description,
            CreatedAt = now,
            Questions = input.Questions?.ToList() ?? new List<string>()
        };

        return _repo.Add(entity);
    }

    public bool Update(Guid id, Questionnaire input)
    {
        var existing = _repo.Get(id);
        if (existing is null) return false;

        if (!string.IsNullOrWhiteSpace(input.Title))
            existing.Title = input.Title.Trim();

        existing.Description = input.Description;
        existing.Questions = input.Questions?.ToList() ?? new List<string>();

        return _repo.Update(existing);
    }

    public bool Delete(Guid id) => _repo.Delete(id);
}
