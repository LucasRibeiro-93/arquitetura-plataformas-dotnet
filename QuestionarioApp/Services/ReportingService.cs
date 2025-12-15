using QuestionarioApp.Models;
using QuestionarioApp.Repositories;

namespace QuestionarioApp.Services;

public class ReportingService : IReportingService
{
    private readonly IQuestionnaireRepository _questionnaireRepo;
    private readonly IQuestionRepository _questionRepo;
    private readonly IAnswerRepository _answerRepo;
    private readonly IQuestionnaireAssignmentRepository _assignmentRepo;

    public ReportingService(
        IQuestionnaireRepository questionnaireRepo,
        IQuestionRepository questionRepo,
        IAnswerRepository answerRepo,
        IQuestionnaireAssignmentRepository assignmentRepo)
    {
        _questionnaireRepo = questionnaireRepo;
        _questionRepo = questionRepo;
        _answerRepo = answerRepo;
        _assignmentRepo = assignmentRepo;
    }

    public QuestionnaireSummaryDto? GetQuestionnaireSummary(Guid questionnaireId)
    {
        var questionnaire = _questionnaireRepo.Get(questionnaireId);
        if (questionnaire is null) return null;

        var summary = new QuestionnaireSummaryDto
        {
            QuestionnaireId = questionnaire.Id,
            QuestionnaireTitle = questionnaire.Title
        };

        var questions = _questionRepo.GetByQuestionnaire(questionnaire.Id).ToList();
        var answersByQuestion = questions.ToDictionary(
            q => q.Id,
            q => _answerRepo.GetByQuestion(q.Id).ToList()
        );

        var assignments = _assignmentRepo.GetByQuestionnaire(questionnaire.Id).ToList();
        summary.Assignments = assignments.Count;
        var completedAssignments = assignments.Where(a => a.CompletedAt.HasValue).ToList();
        summary.Completed = completedAssignments.Count;

        foreach (var q in questions.OrderBy(x => x.Order))
        {
            var qSum = new QuestionSummaryDto
            {
                QuestionId = q.Id,
                Text = q.Text
            };

            var opts = answersByQuestion[q.Id];
            var counts = opts.ToDictionary(a => a.Id, _ => 0);

            int responsesCount = 0;

            foreach (var a in completedAssignments)
            {
                var resp = a.Responses.FirstOrDefault(r => r.QuestionId == q.Id);
                if (resp is null)
                {
                    continue; // skipped for this assignment
                }

                if (resp.AnswerId.HasValue && counts.ContainsKey(resp.AnswerId.Value))
                {
                    counts[resp.AnswerId.Value]++;
                    responsesCount++;
                }
                else
                {
                    // Invalid/missing answer selection counts as skipped for multiple-choice
                }
            }

            qSum.Responses = responsesCount;
            qSum.Skipped = summary.Completed - responsesCount;
            qSum.Answers = opts
                .Select(opt => new AnswerCountDto
                {
                    AnswerId = opt.Id,
                    AnswerText = opt.Text,
                    Count = counts.TryGetValue(opt.Id, out var c) ? c : 0
                })
                .ToList();

            summary.Questions.Add(qSum);
        }

        return summary;
    }

    public IEnumerable<QuestionnaireSummaryDto> GetAllQuestionnairesSummaries()
    {
        var all = _questionnaireRepo.GetAll();
        foreach (var qn in all)
        {
            var s = GetQuestionnaireSummary(qn.Id);
            if (s != null) yield return s;
        }
    }
}
