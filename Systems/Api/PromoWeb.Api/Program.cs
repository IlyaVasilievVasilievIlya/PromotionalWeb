using Microsoft.AspNetCore.Identity;
using PromoWeb.Api;
using PromoWeb.Api.Configuration;
using PromoWeb.Context;
using PromoWeb.Services.Settings;
using PromoWeb.Settings;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder();

builder.AddAppLogger();

var services = builder.Services;

services.AddHttpContextAccessor();
services.AddAppCors();

https://stackoverflow.com/questions/65449615/custom-claim-not-accessible-in-authorizationhandlercontext-identity-server-4-jwt
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");
var identitySettings = Settings.Load<IdentitySettings>("Identity");


services.AddAppHealthChecks();
services.AddAppVersioning();
services.AddAppSwagger(identitySettings, swaggerSettings);
services.AddAppAutoMappers();
services.AddAppAuth(identitySettings);
services.AddAppDbContext();
services.RegisterAppServices();
services.AddAppControllerAndViews();

var app = builder.Build();

DbInitializer.Execute(app.Services);
DbSeeder.Execute(app.Services, true);



app.UseAppMiddlewares();
app.UseAppCors();
app.UseAppHealthChecks();
app.UseStaticFiles();
app.UseAppSwagger();
app.UseAppAuth(); //useauthx2

app.UseAppControllerAndViews();
app.Run();


