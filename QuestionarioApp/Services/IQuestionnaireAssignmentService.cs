using QuestionarioApp.Models;

namespace QuestionarioApp.Services;

public interface IQuestionnaireAssignmentService
{
    IEnumerable<QuestionnaireAssignment> GetAll();
    QuestionnaireAssignment? GetById(Guid id);
    IEnumerable<QuestionnaireAssignment> GetByParticipant(Guid participantId);
    IEnumerable<QuestionnaireAssignment> GetByQuestionnaire(Guid questionnaireId);
    QuestionnaireAssignment Create(QuestionnaireAssignment input);
    bool Update(Guid id, QuestionnaireAssignment input);
    bool Delete(Guid id);
    bool Submit(Guid id, IEnumerable<AssignmentResponse> responses);
}
