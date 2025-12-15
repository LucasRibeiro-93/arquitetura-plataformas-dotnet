### QuestionarioApp — Educational Survey API (ASP.NET Core, .NET 10)

Early educational version of a questionnaires/surveys API created by student `Lucas de Souza Ribeiro` for the course `Arquitetura de Plataformas .NET` at `Instituto Infnet`.

This repo demonstrates a clean layering (Controllers → Services → Repositories → Models) using dependency injection and an in-memory data store. It is intended for learning purposes only — no authentication/authorization, no persistence to a real database yet.

#### Status
- Early prototype for coursework
- In-memory repositories (data is lost when the app restarts)
- Public admin endpoints exposed to speed up demos (do not use in production)

---

### Tech stack
- .NET 10
- ASP.NET Core Web API (Controllers)
- C# 14
- In-memory repositories (custom) wired through DI

---

### Core domain
- `Questionnaire`: container for multiple `Question`s
- `Question`: a prompt that belongs to a questionnaire and has an `Order`
- `Answer`: one selectable option per question (multiple-choice style for this version)
- `Participant`: a person invited to answer
- `QuestionnaireAssignment`: assigns a questionnaire to a participant and stores their `Responses`
  - `AssignmentResponse`: contains `QuestionId` and the chosen `AnswerId` (nullable for skipped)

---

### Architecture at a glance
- Controllers (HTTP) — input/output boundaries and routing
- Services (business logic) — orchestration and rules
- Repositories (in-memory) — data access abstraction
- Models/DTOs — shapes used across layers

Dependency injection is configured in `Program.cs`:
- Repositories are `Singleton` (shared in-memory state for the app lifetime)
- Services are `Scoped`
- Reporting and Demo seeding services are `Scoped`

---

### Getting started
Prerequisites:
- .NET SDK 10 installed

Run the API locally:
```
cd QuestionarioApp
dotnet run --urls http://localhost:5000
```

If you prefer HTTPS, use `https://localhost:5001` and adjust examples accordingly.

The included Postman collection is at:
`QuestionarioApp/Postman/QuestionarioApp.postman_collection.json`

Set the `baseUrl` variable to your chosen URL (defaults to `http://localhost:5000`).

---

### Seeding demo data (Admin)
For quick demonstrations, use the admin seeding endpoint to populate the in-memory store with deterministic test data.

Endpoint:
- `POST /api/admin/seed/demo`

Query parameters (all optional, defaults shown):
- `reset=true` — clears current in-memory data first
- `seed=42` — set to get deterministic randomization
- `questionnaires=3`
- `questionsPerQuestionnaire=6`
- `answersPerQuestion=4`
- `participants=20`
- `assignments=60` — number of participant-to-questionnaire assignments
- `completionRate=0.7` — fraction (0..1) of assignments marked as completed

Example (curl):
```
curl -X POST "http://localhost:5000/api/admin/seed/demo?reset=true&seed=42&questionnaires=3&questionsPerQuestionnaire=6&answersPerQuestion=4&participants=20&assignments=60&completionRate=0.7"
```

Response shape (`DemoSeedResult`):
```
{
  "questionnaires": 3,
  "questions": 18,
  "answers": 72,
  "participants": 20,
  "assignments": 60,
  "completedAssignments": 42
}
```

---

### Reporting (Admin)
These endpoints summarize responses by questionnaire. They are read-only and work best after using the seed endpoint above.

Endpoints:
- `GET /api/admin/reports/questionnaires/summary` — all questionnaires (list of summaries)
- `GET /api/admin/reports/questionnaires/{id}/summary` — single questionnaire by `Guid`

Response DTOs:
- `QuestionnaireSummaryDto`
  - `questionnaireId` (Guid)
  - `questionnaireTitle` (string)
  - `assignments` (int) — total assignments created for this questionnaire
  - `completed` (int) — how many assignments were completed
  - `questions` (array of `QuestionSummaryDto`)
- `QuestionSummaryDto`
  - `questionId` (Guid)
  - `text` (string)
  - `responses` (int) — number of completed assignments that selected a valid answer
  - `skipped` (int) — `completed - responses`
  - `answers` (array of `AnswerCountDto`)
- `AnswerCountDto`
  - `answerId` (Guid)
  - `answerText` (string)
  - `count` (int)

Example — all questionnaires summary:
```
GET http://localhost:5000/api/admin/reports/questionnaires/summary
```
Sample response (truncated):
```
[
  {
    "questionnaireId": "...",
    "questionnaireTitle": "Questionnaire 1",
    "assignments": 20,
    "completed": 14,
    "questions": [
      {
        "questionId": "...",
        "text": "Q1. Demo question text?",
        "responses": 14,
        "skipped": 0,
        "answers": [
          { "answerId": "...", "answerText": "Option 1", "count": 4 },
          { "answerId": "...", "answerText": "Option 2", "count": 3 },
          { "answerId": "...", "answerText": "Option 3", "count": 5 },
          { "answerId": "...", "answerText": "Option 4", "count": 2 }
        ]
      }
    ]
  }
]
```

---

### Expected workflow for this prototype
1) Seed demo data using the Admin endpoint
2) Explore summaries via the Admin reports endpoints
3) Optionally, use the CRUD endpoints for entities (Questionnaires, Questions, Answers, Participants, Assignments) to inspect or extend data
4) Rerun seeding with different parameters (`seed`, `completionRate`, etc.) to observe report changes

Note: Since storage is in-memory, you start fresh on each application restart unless you seed again.

---

### CRUD endpoints (overview)
The project includes standard REST-style controllers for the following entities:
- `QuestionnairesController` — manage questionnaires
- `QuestionsController` — manage questions of a questionnaire
- `AnswersController` — manage answer options of a question
- `ParticipantsController` — manage participants
- `QuestionnaireAssignmentsController` — assign questionnaires to participants and submit responses

Routes follow the conventional pattern (e.g., `GET/POST/PUT/DELETE`) and operate on the in-memory repositories. See controllers in `QuestionarioApp/Controllers` for exact shapes and parameters.

---

### Postman collection
Import `QuestionarioApp/Postman/QuestionarioApp.postman_collection.json` and set `baseUrl` to your local URL.

Useful requests included:
- Admin → Seed Demo Data (pre-filled with typical parameters)
- Admin → Reports (all summaries and per-questionnaire)

---

### Roadmap (next steps for the ideal project)
- Replace in-memory repositories with a real database (EF Core)
- Add authentication/authorization and proper admin separation
- Validation, error handling, and OpenAPI/Swagger docs
- Pagination and filtering for list endpoints
- Anonymous submission tokens/links per assignment
- More question types (text, scale, checkbox with multiple selections)
- Export reports (CSV/Excel) and richer analytics

---

### Author
- Student: `Lucas de Souza Ribeiro`
- Course: `Arquitetura de Plataformas .NET` — `Instituto Infnet`

Feedback and PRs are welcome for learning purposes.