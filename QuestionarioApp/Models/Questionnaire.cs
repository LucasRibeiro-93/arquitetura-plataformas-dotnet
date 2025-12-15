using System.Collections.Concurrent;

namespace QuestionarioApp.Models;

public class Questionnaire
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Questions { get; set; } = new();
}
