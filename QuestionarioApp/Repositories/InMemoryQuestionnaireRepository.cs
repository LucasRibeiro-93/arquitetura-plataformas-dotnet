using System.Collections.Concurrent;
using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public class InMemoryQuestionnaireRepository : IQuestionnaireRepository
{
    private readonly ConcurrentDictionary<Guid, Questionnaire> _store = new();

    public IEnumerable<Questionnaire> GetAll() => _store.Values.OrderBy(q => q.CreatedAt);

    public Questionnaire? Get(Guid id) => _store.TryGetValue(id, out var q) ? q : null;

    public Questionnaire Add(Questionnaire entity)
    {
        _store[entity.Id] = entity;
        return entity;
    }

    public bool Update(Questionnaire entity)
    {
        if (!_store.ContainsKey(entity.Id)) return false;
        _store[entity.Id] = entity;
        return true;
        
    }

    public bool Delete(Guid id)
    {
        return _store.TryRemove(id, out _);
    }
}
