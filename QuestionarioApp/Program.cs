using QuestionarioApp.Repositories;
using QuestionarioApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Dependency Injection registrations
builder.Services.AddSingleton<IQuestionnaireRepository, InMemoryQuestionnaireRepository>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();

// Questions
builder.Services.AddSingleton<IQuestionRepository, InMemoryQuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();

// Answers
builder.Services.AddSingleton<IAnswerRepository, InMemoryAnswerRepository>();
builder.Services.AddScoped<IAnswerService, AnswerService>();

// Participants
builder.Services.AddSingleton<IParticipantRepository, InMemoryParticipantRepository>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();

// Questionnaire Assignments
builder.Services.AddSingleton<IQuestionnaireAssignmentRepository, InMemoryQuestionnaireAssignmentRepository>();
builder.Services.AddScoped<IQuestionnaireAssignmentService, QuestionnaireAssignmentService>();

// Reporting & Demo seeding
builder.Services.AddScoped<IReportingService, ReportingService>();
builder.Services.AddScoped<IDemoSeeder, DemoSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();