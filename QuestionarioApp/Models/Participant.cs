namespace QuestionarioApp.Models;

public class Participant
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
}
