using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public interface IQuestionRepository
{
    IEnumerable<Question> GetAll();
    Question? Get(Guid id);
    IEnumerable<Question> GetByQuestionnaire(Guid questionnaireId);
    Question Add(Question entity);
    bool Update(Question entity);
    bool Delete(Guid id);
}
