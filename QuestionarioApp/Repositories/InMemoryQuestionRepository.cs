using System.Collections.Concurrent;
using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public class InMemoryQuestionRepository : IQuestionRepository
{
    private readonly ConcurrentDictionary<Guid, Question> _store = new();

    public IEnumerable<Question> GetAll() => _store.Values.OrderBy(q => q.Order);

    public Question? Get(Guid id) => _store.TryGetValue(id, out var q) ? q : null;

    public IEnumerable<Question> GetByQuestionnaire(Guid questionnaireId)
        => _store.Values.Where(q => q.QuestionnaireId == questionnaireId).OrderBy(q => q.Order);

    public Question Add(Question entity)
    {
        _store[entity.Id] = entity;
        return entity;
    }

    public bool Update(Question entity)
    {
        if (!_store.ContainsKey(entity.Id)) return false;
        _store[entity.Id] = entity;
        return true;
    }

    public bool Delete(Guid id) => _store.TryRemove(id, out _);
}
