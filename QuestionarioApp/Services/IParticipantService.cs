using QuestionarioApp.Models;

namespace QuestionarioApp.Services;

public interface IParticipantService
{
    IEnumerable<Participant> GetAll();
    Participant? GetById(Guid id);
    Participant Create(Participant input);
    bool Update(Guid id, Participant input);
    bool Delete(Guid id);
}
