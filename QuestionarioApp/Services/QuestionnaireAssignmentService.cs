using QuestionarioApp.Models;
using QuestionarioApp.Repositories;

namespace QuestionarioApp.Services;

public class QuestionnaireAssignmentService : IQuestionnaireAssignmentService
{
    private readonly IQuestionnaireAssignmentRepository _repo;
    private readonly IParticipantRepository _participantRepo;
    private readonly IQuestionnaireRepository _questionnaireRepo;
    private readonly IQuestionRepository _questionRepo;

    public QuestionnaireAssignmentService(
        IQuestionnaireAssignmentRepository repo,
        IParticipantRepository participantRepo,
        IQuestionnaireRepository questionnaireRepo,
        IQuestionRepository questionRepo)
    {
        _repo = repo;
        _participantRepo = participantRepo;
        _questionnaireRepo = questionnaireRepo;
        _questionRepo = questionRepo;
    }

    public IEnumerable<QuestionnaireAssignment> GetAll() => _repo.GetAll();

    public QuestionnaireAssignment? GetById(Guid id) => _repo.Get(id);

    public IEnumerable<QuestionnaireAssignment> GetByParticipant(Guid participantId) => _repo.GetByParticipant(participantId);

    public IEnumerable<QuestionnaireAssignment> GetByQuestionnaire(Guid questionnaireId) => _repo.GetByQuestionnaire(questionnaireId);

    public QuestionnaireAssignment Create(QuestionnaireAssignment input)
    {
        if (input.QuestionnaireId == Guid.Empty) throw new ArgumentException("QuestionnaireId is required", nameof(input.QuestionnaireId));
        if (input.ParticipantId == Guid.Empty) throw new ArgumentException("ParticipantId is required", nameof(input.ParticipantId));
        if (_participantRepo.Get(input.ParticipantId) is null) throw new ArgumentException("Participant does not exist", nameof(input.ParticipantId));
        if (_questionnaireRepo.Get(input.QuestionnaireId) is null) throw new ArgumentException("Questionnaire does not exist", nameof(input.QuestionnaireId));

        var entity = new QuestionnaireAssignment
        {
            Id = input.Id == Guid.Empty ? Guid.NewGuid() : input.Id,
            QuestionnaireId = input.QuestionnaireId,
            ParticipantId = input.ParticipantId,
            AssignedAt = input.AssignedAt == default ? DateTime.UtcNow : input.AssignedAt,
            CompletedAt = null,
            Responses = new List<AssignmentResponse>()
        };
        return _repo.Add(entity);
    }

    public bool Update(Guid id, QuestionnaireAssignment input)
    {
        var existing = _repo.Get(id);
        if (existing is null) return false;
        if (input.ParticipantId != Guid.Empty)
        {
            if (_participantRepo.Get(input.ParticipantId) is null) return false;
            existing.ParticipantId = input.ParticipantId;
        }
        if (input.QuestionnaireId != Guid.Empty)
        {
            if (_questionnaireRepo.Get(input.QuestionnaireId) is null) return false;
            existing.QuestionnaireId = input.QuestionnaireId;
        }
        if (input.AssignedAt != default) existing.AssignedAt = input.AssignedAt;
        existing.CompletedAt = input.CompletedAt;
        if (input.Responses is { Count: > 0 })
        {
            // validate questions belong to the questionnaire
            var allowedQuestions = _questionRepo.GetByQuestionnaire(existing.QuestionnaireId).Select(q => q.Id).ToHashSet();
            if (input.Responses.Any(r => !allowedQuestions.Contains(r.QuestionId))) return false;
            existing.Responses = input.Responses.ToList();
        }
        return _repo.Update(existing);
    }

    public bool Delete(Guid id) => _repo.Delete(id);

    public bool Submit(Guid id, IEnumerable<AssignmentResponse> responses)
    {
        var existing = _repo.Get(id);
        if (existing is null) return false;
        var allowedQuestions = _questionRepo.GetByQuestionnaire(existing.QuestionnaireId).Select(q => q.Id).ToHashSet();
        var respList = responses?.ToList() ?? new List<AssignmentResponse>();
        if (respList.Any(r => !allowedQuestions.Contains(r.QuestionId))) return false;
        existing.Responses = respList;
        existing.CompletedAt = DateTime.UtcNow;
        return _repo.Update(existing);
    }
}
