using QuestionarioApp.Models;

namespace QuestionarioApp.Services;

public interface IAnswerService
{
    IEnumerable<Answer> GetAll();
    Answer? GetById(Guid id);
    IEnumerable<Answer> GetByQuestion(Guid questionId);
    Answer Create(Answer input);
    bool Update(Guid id, Answer input);
    bool Delete(Guid id);
}
