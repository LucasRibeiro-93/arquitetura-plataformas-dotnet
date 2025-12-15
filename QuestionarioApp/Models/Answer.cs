namespace QuestionarioApp.Models;

public class Answer
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid QuestionId { get; set; } = Guid.Empty;
    public string Text { get; set; } = string.Empty;
}
