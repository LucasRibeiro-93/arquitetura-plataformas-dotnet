using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public interface IQuestionnaireRepository
{
    IEnumerable<Questionnaire> GetAll();
    Questionnaire? Get(Guid id);
    Questionnaire Add(Questionnaire entity);
    bool Update(Questionnaire entity);
    bool Delete(Guid id);
}
