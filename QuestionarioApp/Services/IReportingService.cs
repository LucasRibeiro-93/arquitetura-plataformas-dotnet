using QuestionarioApp.Models;

namespace QuestionarioApp.Services;

public class AnswerCountDto
{
    public Guid AnswerId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class QuestionSummaryDto
{
    public Guid QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Responses { get; set; }
    public int Skipped { get; set; }
    public List<AnswerCountDto> Answers { get; set; } = new();
}

public class QuestionnaireSummaryDto
{
    public Guid QuestionnaireId { get; set; }
    public string QuestionnaireTitle { get; set; } = string.Empty;
    public int Assignments { get; set; }
    public int Completed { get; set; }
    public List<QuestionSummaryDto> Questions { get; set; } = new();
}

public interface IReportingService
{
    QuestionnaireSummaryDto? GetQuestionnaireSummary(Guid questionnaireId);
    IEnumerable<QuestionnaireSummaryDto> GetAllQuestionnairesSummaries();
}
