using FluentValidation;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.Diagnostic;
using SyzygyVeterinaryAPIControllersData.Repositories.ExamAnalyses;
using SyzygyVeterinaryAPIControllersData.Repositories.ReferenceValues;
using SyzygyVeterinaryAPIControllersData.Repositories.Veterinaries;
using SyzygyVeterinaryAPIControllersData.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDbDataAccess, DbDataAccess>();
builder.Services.AddScoped<IExamAnalysisRepository, ExamAnalysisRepository>();
builder.Services.AddScoped<IReferenceValueRepository, ReferenceValueRepository>();
builder.Services.AddScoped<IVeterinariansRepository, VeterinariansRepository>();
builder.Services.AddScoped<IDiagnosticsRepository, DiagnosticsRepository>();

// Validation
builder.Services.AddScoped<IValidator<ExamAnalysisModel>, ExamAnalysisValidator>();
builder.Services.AddScoped<IValidator<ReferenceValueModel>, ReferenceValueValidator>();
builder.Services.AddScoped<IValidator<DiagnosticsModel>, DiagnosticsValidator>();
builder.Services.AddScoped<IValidator<VeterinariansModel>, VeterinariansValidator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
