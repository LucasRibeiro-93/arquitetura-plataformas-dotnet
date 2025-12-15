using QuestionarioApp.Models;
using QuestionarioApp.Repositories;

namespace QuestionarioApp.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _repo;
    private readonly IQuestionnaireRepository _questionnaireRepo;

    public QuestionService(IQuestionRepository repo, IQuestionnaireRepository questionnaireRepo)
    {
        _repo = repo;
        _questionnaireRepo = questionnaireRepo;
    }

    public IEnumerable<Question> GetAll() => _repo.GetAll();

    public Question? GetById(Guid id) => _repo.Get(id);

    public IEnumerable<Question> GetByQuestionnaire(Guid questionnaireId) => _repo.GetByQuestionnaire(questionnaireId);

    public Question Create(Question input)
    {
        if (input.QuestionnaireId == Guid.Empty) throw new ArgumentException("QuestionnaireId is required", nameof(input.QuestionnaireId));
        if (string.IsNullOrWhiteSpace(input.Text)) throw new ArgumentException("Text is required", nameof(input.Text));
        if (_questionnaireRepo.Get(input.QuestionnaireId) is null) throw new ArgumentException("Questionnaire does not exist", nameof(input.QuestionnaireId));

        var entity = new Question
        {
            Id = input.Id == Guid.Empty ? Guid.NewGuid() : input.Id,
            QuestionnaireId = input.QuestionnaireId,
            Text = input.Text.Trim(),
            Order = input.Order
        };
        return _repo.Add(entity);
    }

    public bool Update(Guid id, Question input)
    {
        var existing = _repo.Get(id);
        if (existing is null) return false;
        if (!string.IsNullOrWhiteSpace(input.Text)) existing.Text = input.Text.Trim();
        if (input.Order != default) existing.Order = input.Order;
        if (input.QuestionnaireId != Guid.Empty)
        {
            if (_questionnaireRepo.Get(input.QuestionnaireId) is null) return false;
            existing.QuestionnaireId = input.QuestionnaireId;
        }
        return _repo.Update(existing);
    }

    public bool Delete(Guid id) => _repo.Delete(id);
}
