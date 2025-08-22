using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Quiz;
using Quiz.Features.Quizzes.Services;
using Quiz.Features.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:3002") });
builder.Services.AddScoped<CustomHttpClient>();
builder.Services.AddScoped<FormService>();

await builder.Build().RunAsync();
