using QuestionarioApp.Models;

namespace QuestionarioApp.Services;

public interface IQuestionnaireService
{
    IEnumerable<Questionnaire> GetAll();
    Questionnaire? GetById(Guid id);
    Questionnaire Create(Questionnaire input);
    bool Update(Guid id, Questionnaire input);
    bool Delete(Guid id);
}
