using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using BookStoreApi.Configurations;
using BookStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add configurations
builder.Services.Configure<BookStoreDatabaseConfig>(
    builder.Configuration.GetSection("BookStoreDatabase"));

// Add services to the container.
builder.Services.AddSingleton<BooksService>();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Dependency Injection
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TodoApi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1"));
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
