using QuestionarioApp.Models;

namespace QuestionarioApp.Services;

public interface IQuestionService
{
    IEnumerable<Question> GetAll();
    Question? GetById(Guid id);
    IEnumerable<Question> GetByQuestionnaire(Guid questionnaireId);
    Question Create(Question input);
    bool Update(Guid id, Question input);
    bool Delete(Guid id);
}
