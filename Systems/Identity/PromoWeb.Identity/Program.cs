using PromoWeb.Context;
using PromoWeb.Identity;
using PromoWeb.Identity.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddAppCors();

services.AddAppHealthChecks(); 
services.AddAppDbContext(builder.Configuration); //сервер будет работать со списком пользователей из базы

services.AddIS4();

services.RegisterAppServices(); //пусто по сервисам
var app = builder.Build();

app.UseAppCors();

app.UseAppHealthChecks();
app.UseIS4();
app.Run();

