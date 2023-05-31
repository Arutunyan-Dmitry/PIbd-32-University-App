using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using M6T.Core.TupleModelBinder;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using UniversityBusinessLogic.BusinessLogic;
using UniversityBusinessLogic.MailWorker;
using UniversityBusinessLogic.OfficePackage;
using UniversityBusinessLogic.OfficePackage.Implements;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.StorageContracts;
using UniversityDatabaseImplement.Implements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Services connection
builder.Services.AddTransient<IDepartmentStorage, DepartmentStorage>();
builder.Services.AddTransient<IDisciplineStorage, DisciplineStorage>();
builder.Services.AddTransient<IGroupStorage, GroupStorage>();
builder.Services.AddTransient<IPlanStorage, PlanStorage>();
builder.Services.AddTransient<IStudentStorage, StudentStorage>();
builder.Services.AddTransient<ITeacherStorage, TeacherStorage>();
builder.Services.AddTransient<ITestingStorage, TestingStorage>();
builder.Services.AddTransient<IMessageStorage, MessageStorage>();

builder.Services.AddTransient<IDepartmentLogic, DepartmentLogic>();
builder.Services.AddTransient<IDisciplineLogic, DisciplineLogic>();
builder.Services.AddTransient<IGroupLogic, GroupLogic>();
builder.Services.AddTransient<IPlanLogic, PlanLogic>();
builder.Services.AddTransient<IStudentLogic, StudentLogic>();
builder.Services.AddTransient<ITeacherLogic, TeacherLogic>();
builder.Services.AddTransient<ITestingLogic, TestingLogic>();
builder.Services.AddTransient<IMessageLogic, MessageLogic>();
builder.Services.AddTransient<IReportLogic, ReportLogic>();
builder.Services.AddTransient<AbstractSaveToWord, SaveToWord>();
builder.Services.AddSingleton<AbstractMailWorker, MailKitWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App builder options
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniversityWebApi", Version = "v1" });
});
builder.Services.AddMvc(options =>
{
    options.ModelBinderProviders.Insert(0, new TupleModelBinderProvider());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniversityWebApi v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

var mailSender = app.Services.GetService<AbstractMailWorker>();
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
mailSender.MailConfig(new MailConfigBindingModel
{
    MailLogin = config?.GetSection("MailLogin")?.Value.ToString(),
    MailPassword = config?.GetSection("MailPassword")?.Value.ToString(),
    SmtpClientHost = config?.GetSection("SmtpClientHost")?.Value.ToString(),
    SmtpClientPort = Convert.ToInt32(config?.GetSection("SmtpClientPort")?.Value.ToString()),
    PopHost = config?.GetSection("PopHost")?.Value.ToString(),
    PopPort = Convert.ToInt32(config?.GetSection("PopPort")?.Value.ToString())
});

app.Run();
