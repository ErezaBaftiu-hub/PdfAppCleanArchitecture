using Application;
using Application.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfApi.Middleware;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var _configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PdfDbContext>(options =>
                options.UseSqlServer(_configuration["ConnectionStrings:PdfDbContext"]));

builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
builder.Services.AddScoped<IValidator<PdfInputModel>, PdfInputModelValidator>();
builder.Services.AddAutoMapper(typeof(PdfMapper));
builder.Services.AddAuthorization();

var elasticSearchUrl = _configuration["Elastic"];

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Debug)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Debug)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
    {
        AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        IndexFormat = $"server log-{DateTime.Now:yyyy.MM.dd}"

    })
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ApiKeyMiddleware>();
app.MapGet("/pdf/{id}", async (int id, IPdfService _pdfService) =>
{
    var pdfModel = await _pdfService.GetByIdAsync(id);
    return Results.Json(pdfModel);
});
app.MapPost("/pdf", async (IValidator<PdfInputModel> validator, [FromBody] PdfInputModel pdfInputModel, IPdfService _pdfService) =>
{
    ValidationResult validationResult = await validator.ValidateAsync(pdfInputModel);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var pdfOutputModel = await _pdfService.ConvertHtmlToPdfAsync(pdfInputModel);
    return Results.Json(pdfOutputModel);
});

app.Run();
