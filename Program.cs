using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Quiz;
using Quiz.Common.Services;
using Quiz.Features.Quizzes.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:3002") });
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<CustomHttpClient>();
builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<ThemeService>();

await builder.Build().RunAsync();
