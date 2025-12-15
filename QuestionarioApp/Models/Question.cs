namespace QuestionarioApp.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid QuestionnaireId { get; set; } = Guid.Empty;
    public string Text { get; set; } = string.Empty;
    public int Order { get; set; }
}
