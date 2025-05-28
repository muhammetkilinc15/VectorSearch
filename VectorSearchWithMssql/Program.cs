using VectorSearchWithMssql.Services;
using System.ComponentModel.DataAnnotations;
using VectorSearchWithMssql.Dtos;
using VectorSearchWithMssql.Context;
using Microsoft.EntityFrameworkCore;
using VectorSearchWithMssql.Extensions;
using VectorSearchWithMssql.Exceptions;

var builder = WebApplication.CreateBuilder(args);


// Add services from extensions
builder.Services.AddOllamaEmbedding(builder.Configuration);


builder.Services.AddControllers();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), o => o.UseVectorSearch()));


builder.Services.AddScoped<IOllamaEmbeddingService, OllamaEmbeddingService>();


var app = builder.Build();



app.UseExceptionHandler();

app.UseCors("CorsPolicy");

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();


app.MapControllers();

app.Run();