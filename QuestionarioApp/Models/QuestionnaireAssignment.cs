namespace QuestionarioApp.Models;

public class QuestionnaireAssignment
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid QuestionnaireId { get; set; } = Guid.Empty;
    public Guid ParticipantId { get; set; } = Guid.Empty;
    public DateTime AssignedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<AssignmentResponse> Responses { get; set; } = new();
}

public class AssignmentResponse
{
    public Guid QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
}
