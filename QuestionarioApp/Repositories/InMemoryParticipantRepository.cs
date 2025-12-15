using System.Collections.Concurrent;
using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public class InMemoryParticipantRepository : IParticipantRepository
{
    private readonly ConcurrentDictionary<Guid, Participant> _store = new();

    public IEnumerable<Participant> GetAll() => _store.Values.OrderBy(p => p.Name);

    public Participant? Get(Guid id) => _store.TryGetValue(id, out var p) ? p : null;

    public Participant Add(Participant entity)
    {
        _store[entity.Id] = entity;
        return entity;
    }

    public bool Update(Participant entity)
    {
        if (!_store.ContainsKey(entity.Id)) return false;
        _store[entity.Id] = entity;
        return true;
    }

    public bool Delete(Guid id) => _store.TryRemove(id, out _);
}
