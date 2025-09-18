using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Quiz;
using Quiz.Common.Services;
using Quiz.Features.Quizzes.Services;
using Quiz.Common.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app", options => options.UseHashRouting = true);
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:3002") });
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<CustomHttpClient>();
builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<IErrorLoggerService, ErrorLoggerService>();

await builder.Build().RunAsync();
