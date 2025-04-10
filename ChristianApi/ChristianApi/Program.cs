using ChristianApi.Data;
using ChristianApi.Data.Interfaces;
using ChristianApi.Models;
using ChristianApi.Services;
using ChristianApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenAI.ChatGpt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBibleVerseService, BibleVerseServices>();
builder.Services.AddScoped<IBibleVerseData, BibleVerseData>();
builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<ISermonService, SermonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();
//app.UseRouting();
app.UseAuthorization();
//app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
