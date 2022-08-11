using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using DotNetRestApi.Models;
using DotNetRestApi.Configurations;
using DotNetRestApi.Services;
using DotNetRestApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection

// Add configurations
builder.Services.Configure<BookStoreDatabaseConfig>(
    builder.Configuration.GetSection("BookStoreDatabase"));

// Add services to the container.
builder.Services.AddSingleton<BooksService>();
// Add DbContext
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new() { Title = "DotNetRestApi", Version = "v1" });
});

builder.Services.AddHttpClient("BLOCKPASS", httpClient =>
{
  const string BLOCKPASS_API_KEY = "";
  const string BLOCKPASS_CLIENT_ID = "";
  httpClient.BaseAddress = new Uri($"https://kyc.blockpass.org/kyc/1.0/connect/{BLOCKPASS_CLIENT_ID}/");
  httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, BLOCKPASS_API_KEY);
  httpClient.DefaultRequestHeaders.Add(HeaderNames.CacheControl, "no-cache");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  //   app.UseSwaggerUI();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetRestApi v1"));
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWhen(
    ctx => ctx.Request.Path.StartsWithSegments("/api/Callback"),
    ab => ab.UseMiddleware<EnableRequestBodyBufferingMiddleware>());

app.MapControllers();

app.Run();
