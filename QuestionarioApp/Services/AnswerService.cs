using QuestionarioApp.Models;
using QuestionarioApp.Repositories;

namespace QuestionarioApp.Services;

public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _repo;
    private readonly IQuestionRepository _questionRepo;

    public AnswerService(IAnswerRepository repo, IQuestionRepository questionRepo)
    {
        _repo = repo;
        _questionRepo = questionRepo;
    }

    public IEnumerable<Answer> GetAll() => _repo.GetAll();

    public Answer? GetById(Guid id) => _repo.Get(id);

    public IEnumerable<Answer> GetByQuestion(Guid questionId) => _repo.GetByQuestion(questionId);

    public Answer Create(Answer input)
    {
        if (input.QuestionId == Guid.Empty) throw new ArgumentException("QuestionId is required", nameof(input.QuestionId));
        if (string.IsNullOrWhiteSpace(input.Text)) throw new ArgumentException("Text is required", nameof(input.Text));
        if (_questionRepo.Get(input.QuestionId) is null) throw new ArgumentException("Question does not exist", nameof(input.QuestionId));

        var entity = new Answer
        {
            Id = input.Id == Guid.Empty ? Guid.NewGuid() : input.Id,
            QuestionId = input.QuestionId,
            Text = input.Text.Trim()
        };
        return _repo.Add(entity);
    }

    public bool Update(Guid id, Answer input)
    {
        var existing = _repo.Get(id);
        if (existing is null) return false;
        if (!string.IsNullOrWhiteSpace(input.Text)) existing.Text = input.Text.Trim();
        if (input.QuestionId != Guid.Empty)
        {
            if (_questionRepo.Get(input.QuestionId) is null) return false;
            existing.QuestionId = input.QuestionId;
        }
        return _repo.Update(existing);
    }

    public bool Delete(Guid id) => _repo.Delete(id);
}
