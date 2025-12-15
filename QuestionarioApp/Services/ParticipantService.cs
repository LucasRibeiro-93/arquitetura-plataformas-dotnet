using QuestionarioApp.Models;
using QuestionarioApp.Repositories;

namespace QuestionarioApp.Services;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _repo;

    public ParticipantService(IParticipantRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Participant> GetAll() => _repo.GetAll();

    public Participant? GetById(Guid id) => _repo.Get(id);

    public Participant Create(Participant input)
    {
        if (string.IsNullOrWhiteSpace(input.Name)) throw new ArgumentException("Name is required", nameof(input.Name));

        var entity = new Participant
        {
            Id = input.Id == Guid.Empty ? Guid.NewGuid() : input.Id,
            Name = input.Name.Trim(),
            Email = string.IsNullOrWhiteSpace(input.Email) ? null : input.Email!.Trim()
        };

        return _repo.Add(entity);
    }

    public bool Update(Guid id, Participant input)
    {
        var existing = _repo.Get(id);
        if (existing is null) return false;
        if (!string.IsNullOrWhiteSpace(input.Name)) existing.Name = input.Name.Trim();
        existing.Email = string.IsNullOrWhiteSpace(input.Email) ? existing.Email : input.Email!.Trim();
        return _repo.Update(existing);
    }

    public bool Delete(Guid id) => _repo.Delete(id);
}
