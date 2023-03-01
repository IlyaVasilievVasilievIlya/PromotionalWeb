#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Worker/PromoWeb.Worker/PromoWeb.Worker.csproj", "Systems/Worker/PromoWeb.Worker/"]
COPY ["Services/PromoWeb.Services.EmailSender/PromoWeb.Services.EmailSender.csproj", "Services/PromoWeb.Services.EmailSender/"]
COPY ["Shared/PromoWeb.Settings/PromoWeb.Settings.csproj", "Shared/PromoWeb.Settings/"]
COPY ["Services/PromoWeb.Services.RabbitMq/PromoWeb.Services.RabbitMq.csproj", "Services/PromoWeb.Services.RabbitMq/"]
COPY ["Shared/PromoWeb.Common/PromoWeb.Common.csproj", "Shared/PromoWeb.Common/"]
COPY ["Shared/PromoWeb.Consts/PromoWeb.Consts.csproj", "Shared/PromoWeb.Consts/"]
RUN dotnet restore "Systems/Worker/PromoWeb.Worker/PromoWeb.Worker.csproj"
COPY . .
WORKDIR "/src/Systems/Worker/PromoWeb.Worker"
RUN dotnet build "PromoWeb.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PromoWeb.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PromoWeb.Worker.dll"]