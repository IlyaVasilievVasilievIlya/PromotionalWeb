using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PromoWeb.Web;
using PromoWeb.Web.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IAppInfoService, AppInfoService>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

await builder.Build().RunAsync();
