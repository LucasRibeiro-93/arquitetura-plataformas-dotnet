using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public interface IAnswerRepository
{
    IEnumerable<Answer> GetAll();
    Answer? Get(Guid id);
    IEnumerable<Answer> GetByQuestion(Guid questionId);
    Answer Add(Answer entity);
    bool Update(Answer entity);
    bool Delete(Guid id);
}
