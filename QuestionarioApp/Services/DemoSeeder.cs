using QuestionarioApp.Models;

namespace QuestionarioApp.Services;

public class DemoSeedResult
{
    public int Questionnaires { get; set; }
    public int Questions { get; set; }
    public int Answers { get; set; }
    public int Participants { get; set; }
    public int Assignments { get; set; }
    public int CompletedAssignments { get; set; }
}

public interface IDemoSeeder
{
    DemoSeedResult SeedDemo(bool reset = true, int? seed = null,
        int questionnaires = 3, int questionsPerQuestionnaire = 6, int answersPerQuestion = 4,
        int participants = 20, int assignments = 60, double completionRate = 0.7);
}

public class DemoSeeder : IDemoSeeder
{
    private readonly IQuestionnaireService _questionnaireService;
    private readonly IQuestionService _questionService;
    private readonly IAnswerService _answerService;
    private readonly IParticipantService _participantService;
    private readonly IQuestionnaireAssignmentService _assignmentService;

    public DemoSeeder(
        IQuestionnaireService questionnaireService,
        IQuestionService questionService,
        IAnswerService answerService,
        IParticipantService participantService,
        IQuestionnaireAssignmentService assignmentService)
    {
        _questionnaireService = questionnaireService;
        _questionService = questionService;
        _answerService = answerService;
        _participantService = participantService;
        _assignmentService = assignmentService;
    }

    public DemoSeedResult SeedDemo(bool reset = true, int? seed = null,
        int questionnaires = 3, int questionsPerQuestionnaire = 6, int answersPerQuestion = 4,
        int participants = 20, int assignments = 60, double completionRate = 0.7)
    {
        var rng = seed.HasValue ? new Random(seed.Value) : new Random();

        if (reset)
        {
            // Delete assignments first
            foreach (var a in _assignmentService.GetAll().ToList())
                _assignmentService.Delete(a.Id);

            // Then answers
            foreach (var ans in _answerService.GetAll().ToList())
                _answerService.Delete(ans.Id);

            // Then questions
            foreach (var q in _questionService.GetAll().ToList())
                _questionService.Delete(q.Id);

            // Then questionnaires
            foreach (var qn in _questionnaireService.GetAll().ToList())
                _questionnaireService.Delete(qn.Id);

            // Finally participants
            foreach (var p in _participantService.GetAll().ToList())
                _participantService.Delete(p.Id);
        }

        var result = new DemoSeedResult();

        // Create participants
        var participantIds = new List<Guid>();
        for (int i = 1; i <= participants; i++)
        {
            var p = _participantService.Create(new Participant
            {
                Name = $"Participant {i}",
                Email = $"participant{i}@example.com"
            });
            participantIds.Add(p.Id);
        }
        result.Participants = participantIds.Count;

        // Create questionnaires, questions, answers
        var questionnaireIds = new List<Guid>();
        var questionsByQuestionnaire = new Dictionary<Guid, List<Guid>>();
        var answersByQuestion = new Dictionary<Guid, List<Guid>>();

        for (int qi = 1; qi <= questionnaires; qi++)
        {
            var qn = _questionnaireService.Create(new Questionnaire
            {
                Title = $"Questionnaire {qi}",
                Description = $"Demo questionnaire #{qi}"
            });
            questionnaireIds.Add(qn.Id);

            var qIds = new List<Guid>();
            for (int qj = 1; qj <= questionsPerQuestionnaire; qj++)
            {
                var q = _questionService.Create(new Question
                {
                    QuestionnaireId = qn.Id,
                    Text = $"Q{qj}. Demo question text?",
                    Order = qj
                });
                qIds.Add(q.Id);

                var aIds = new List<Guid>();
                for (int ak = 1; ak <= answersPerQuestion; ak++)
                {
                    var ans = _answerService.Create(new Answer
                    {
                        QuestionId = q.Id,
                        Text = $"Option {ak}"
                    });
                    aIds.Add(ans.Id);
                }
                answersByQuestion[q.Id] = aIds;
            }

            questionsByQuestionnaire[qn.Id] = qIds;
        }

        result.Questionnaires = questionnaireIds.Count;
        result.Questions = questionsByQuestionnaire.Sum(kv => kv.Value.Count);
        result.Answers = answersByQuestion.Values.Sum(l => l.Count);

        // Create assignments
        int completed = 0;
        for (int i = 0; i < assignments; i++)
        {
            var qnId = questionnaireIds[rng.Next(questionnaireIds.Count)];
            var partId = participantIds[rng.Next(participantIds.Count)];

            var created = _assignmentService.Create(new QuestionnaireAssignment
            {
                QuestionnaireId = qnId,
                ParticipantId = partId
            });

            // Decide completion
            if (rng.NextDouble() <= completionRate)
            {
                var qIds = questionsByQuestionnaire[qnId];
                var responses = new List<AssignmentResponse>();
                foreach (var qId in qIds)
                {
                    var aIds = answersByQuestion[qId];
                    var pick = aIds[rng.Next(aIds.Count)];
                    responses.Add(new AssignmentResponse
                    {
                        QuestionId = qId,
                        AnswerId = pick
                    });
                }

                _assignmentService.Submit(created.Id, responses);
                completed++;
            }
        }

        result.Assignments = assignments;
        result.CompletedAssignments = completed;
        return result;
    }
}
