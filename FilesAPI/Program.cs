using FilesAPI.Application.File.Handlers;
using FilesAPI.Interfaces;
using FilesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Register Swagger services
builder.Services.AddSwaggerGen();
// DI: register handler and service for dependency injection
builder.Services.AddScoped<MutateFileCommandHandler>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// Enable swagger in dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();