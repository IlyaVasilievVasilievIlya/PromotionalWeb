using PromoWeb.Worker;
using PromoWeb.Worker.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddHttpContextAccessor();

services.AddAppHealthChecks();

services.RegisterAppServices();

var app = builder.Build();

app.UseAppHealthChecks();

app.Services.GetRequiredService<ITaskExecutor>().Start();

app.Run();
