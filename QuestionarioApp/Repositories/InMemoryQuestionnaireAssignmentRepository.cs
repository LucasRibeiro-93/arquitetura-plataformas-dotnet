using System.Collections.Concurrent;
using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public class InMemoryQuestionnaireAssignmentRepository : IQuestionnaireAssignmentRepository
{
    private readonly ConcurrentDictionary<Guid, QuestionnaireAssignment> _store = new();

    public IEnumerable<QuestionnaireAssignment> GetAll() => _store.Values.OrderBy(a => a.AssignedAt);

    public QuestionnaireAssignment? Get(Guid id) => _store.TryGetValue(id, out var a) ? a : null;

    public IEnumerable<QuestionnaireAssignment> GetByParticipant(Guid participantId)
        => _store.Values.Where(a => a.ParticipantId == participantId).OrderBy(a => a.AssignedAt);

    public IEnumerable<QuestionnaireAssignment> GetByQuestionnaire(Guid questionnaireId)
        => _store.Values.Where(a => a.QuestionnaireId == questionnaireId).OrderBy(a => a.AssignedAt);

    public QuestionnaireAssignment Add(QuestionnaireAssignment entity)
    {
        _store[entity.Id] = entity;
        return entity;
    }

    public bool Update(QuestionnaireAssignment entity)
    {
        if (!_store.ContainsKey(entity.Id)) return false;
        _store[entity.Id] = entity;
        return true;
    }

    public bool Delete(Guid id) => _store.TryRemove(id, out _);
}
