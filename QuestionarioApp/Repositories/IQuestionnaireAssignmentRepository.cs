using QuestionarioApp.Models;

namespace QuestionarioApp.Repositories;

public interface IQuestionnaireAssignmentRepository
{
    IEnumerable<QuestionnaireAssignment> GetAll();
    QuestionnaireAssignment? Get(Guid id);
    IEnumerable<QuestionnaireAssignment> GetByParticipant(Guid participantId);
    IEnumerable<QuestionnaireAssignment> GetByQuestionnaire(Guid questionnaireId);
    QuestionnaireAssignment Add(QuestionnaireAssignment entity);
    bool Update(QuestionnaireAssignment entity);
    bool Delete(Guid id);
}
