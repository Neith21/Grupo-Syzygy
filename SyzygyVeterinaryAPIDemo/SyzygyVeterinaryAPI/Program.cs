using FluentValidation;
using SyzygyVeterinaryAPIControllersData.Data;
using SyzygyVeterinaryAPIControllersData.Models;
using SyzygyVeterinaryAPIControllersData.Repositories.AnimalOwners;
using SyzygyVeterinaryAPIControllersData.Repositories.Animals;
using SyzygyVeterinaryAPIControllersData.Repositories.ClinicalExams;
using SyzygyVeterinaryAPIControllersData.Repositories.Diagnostic;
using SyzygyVeterinaryAPIControllersData.Repositories.ExamAnalyses;
using SyzygyVeterinaryAPIControllersData.Repositories.LabTechnicians;
using SyzygyVeterinaryAPIControllersData.Repositories.ReferenceValues;
using SyzygyVeterinaryAPIControllersData.Repositories.Species;
using SyzygyVeterinaryAPIControllersData.Repositories.Veterinaries;
using SyzygyVeterinaryAPIControllersData.Services.AnalisysExams;
using SyzygyVeterinaryAPIControllersData.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDbDataAccess, DbDataAccess>();
builder.Services.AddScoped<IExamAnalysisRepository, ExamAnalysisRepository>();
builder.Services.AddScoped<IReferenceValueRepository, ReferenceValueRepository>();
builder.Services.AddScoped<IVeterinariansRepository, VeterinariansRepository>();
builder.Services.AddScoped<IDiagnosticsRepository, DiagnosticsRepository>();
builder.Services.AddScoped<IAnimalOwnerRepository, AnimalOwnerRepository>();
builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();
builder.Services.AddScoped<ILabTechnicianRepository, LabTechnicianRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IClinicalExamRepository, ClinicalExamRepository>();
builder.Services.AddScoped<IAnalisysExamService, AnalisysExamService>();

// Validation
builder.Services.AddScoped<IValidator<ExamAnalysisModel>, ExamAnalysisValidator>();
builder.Services.AddScoped<IValidator<ReferenceValueModel>, ReferenceValueValidator>();
builder.Services.AddScoped<IValidator<DiagnosticsModel>, DiagnosticsValidator>();
builder.Services.AddScoped<IValidator<VeterinariansModel>, VeterinariansValidator>();
builder.Services.AddScoped<IValidator<AnimalOwnerModel>, AnimalOwnerValidator>();
builder.Services.AddScoped<IValidator<SpeciesModel>, SpeciesValidator>();
builder.Services.AddScoped<IValidator<LabTechnicianModel>, LabTechnicianValidator>();
builder.Services.AddScoped<IValidator<AnimalModel>, AnimalValidator>();
builder.Services.AddScoped<IValidator<ClinicalExamModel>, ClinicalExamValidator>();

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
