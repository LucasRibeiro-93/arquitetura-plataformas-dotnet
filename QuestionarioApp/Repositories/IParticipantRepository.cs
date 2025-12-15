using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public interface IParticipantRepository
{
    IEnumerable<Participant> GetAll();
    Participant? Get(Guid id);
    Participant Add(Participant entity);
    bool Update(Participant entity);
    bool Delete(Guid id);
}
