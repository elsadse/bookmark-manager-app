using bookmark_manager_app.Exceptions.Handler;
using bookmark_manager_app.Exceptions.Handlers;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Persistence;
using bookmark_manager_app.Repositories;
using bookmark_manager_app.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = context.HttpContext.Request.Path;
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow.ToString();
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    };
});

builder.Services.AddExceptionHandler<HandlerValidationException>();
builder.Services.AddExceptionHandler<HandlerNotFoundException>();
builder.Services.AddExceptionHandler<HandlerBadRequestException>();
builder.Services.AddExceptionHandler<HandlerConflictException>();
builder.Services.AddExceptionHandler<HandlerForbiddenException>();
builder.Services.AddExceptionHandler<HandlerGlobalException>();

builder.Services.AddDbContext<BookmarkDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookmarkRepository, BookmarkRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBookmarkTagRepository, BookmarkTagRepository>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();
app.Run();
