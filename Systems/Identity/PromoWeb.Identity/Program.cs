using PromoWeb.Context;
using PromoWeb.Identity;
using PromoWeb.Identity.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddAppCors();

services.AddAppHealthChecks(); 
services.AddAppDbContext(builder.Configuration); //конфигурацию дефолтную подгружает

services.AddIS4();

services.RegisterAppServices();
var app = builder.Build();

app.UseAppCors();



app.UseAppHealthChecks();
app.UseIS4();
SeedUserData.Execute(app.Services);

app.Run();

