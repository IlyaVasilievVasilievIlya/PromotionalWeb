#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Api/PromoWeb.Api/PromoWeb.Api.csproj", "Systems/Api/PromoWeb.Api/"]
COPY ["Data/Migrations/PromoWeb.Context.MigrationsPostgreSQL/PromoWeb.Context.MigrationsPostgreSQL.csproj", "Data/Migrations/PromoWeb.Context.MigrationsPostgreSQL/"]
COPY ["Data/PromoWeb.Context.Entities/PromoWeb.Context.Entities.csproj", "Data/PromoWeb.Context.Entities/"]
COPY ["Data/PromoWeb.Context/PromoWeb.Context.csproj", "Data/PromoWeb.Context/"]
COPY ["Shared/PromoWeb.Settings/PromoWeb.Settings.csproj", "Shared/PromoWeb.Settings/"]
COPY ["Services/PromoWeb.Services.Questions/PromoWeb.Services.Questions.csproj", "Services/PromoWeb.Services.Questions/"]
COPY ["Shared/PromoWeb.Common/PromoWeb.Common.csproj", "Shared/PromoWeb.Common/"]
COPY ["Services/PromoWeb.Services.Actions/PromoWeb.Services.Actions.csproj", "Services/PromoWeb.Services.Actions/"]
COPY ["Shared/PromoWeb.Consts/PromoWeb.Consts.csproj", "Shared/PromoWeb.Consts/"]
COPY ["Services/PromoWeb.Services.EmailSender/PromoWeb.Services.EmailSender.csproj", "Services/PromoWeb.Services.EmailSender/"]
COPY ["Services/PromoWeb.Services.RabbitMq/PromoWeb.Services.RabbitMq.csproj", "Services/PromoWeb.Services.RabbitMq/"]
COPY ["Services/PromoWeb.Services.AppInfos/PromoWeb.Services.AppInfos.csproj", "Services/PromoWeb.Services.AppInfos/"]
COPY ["Services/PromoWeb.Services.Contacts/PromoWeb.Services.Contacts.csproj", "Services/PromoWeb.Services.Contacts/"]
COPY ["Services/PromoWeb.Services.Images/PromoWeb.Services.Images.csproj", "Services/PromoWeb.Services.Images/"]
COPY ["Services/PromoWeb.Services.Links/PromoWeb.Services.Links.csproj", "Services/PromoWeb.Services.Links/"]
COPY ["Services/PromoWeb.Services.Answers/PromoWeb.Services.Answers.csproj", "Services/PromoWeb.Services.Answers/"]
COPY ["Services/PromoWeb.Services.Sections/PromoWeb.Services.Sections.csproj", "Services/PromoWeb.Services.Sections/"]
COPY ["Services/PromoWeb.Services.Settings/PromoWeb.Services.Settings.csproj", "Services/PromoWeb.Services.Settings/"]
COPY ["Services/PromoWeb.Services.UserAccount/PromoWeb.Services.UserAccount.csproj", "Services/PromoWeb.Services.UserAccount/"]
RUN dotnet restore "Systems/Api/PromoWeb.Api/PromoWeb.Api.csproj"
COPY . .
WORKDIR "/src/Systems/Api/PromoWeb.Api"
RUN dotnet build "PromoWeb.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PromoWeb.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PromoWeb.Api.dll"]