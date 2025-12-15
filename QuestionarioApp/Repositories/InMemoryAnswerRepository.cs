using System.Collections.Concurrent;
using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public class InMemoryAnswerRepository : IAnswerRepository
{
    private readonly ConcurrentDictionary<Guid, Answer> _store = new();

    public IEnumerable<Answer> GetAll() => _store.Values;

    public Answer? Get(Guid id) => _store.TryGetValue(id, out var a) ? a : null;

    public IEnumerable<Answer> GetByQuestion(Guid questionId)
        => _store.Values.Where(a => a.QuestionId == questionId);

    public Answer Add(Answer entity)
    {
        _store[entity.Id] = entity;
        return entity;
    }

    public bool Update(Answer entity)
    {
        if (!_store.ContainsKey(entity.Id)) return false;
        _store[entity.Id] = entity;
        return true;
    }

    public bool Delete(Guid id) => _store.TryRemove(id, out _);
}
