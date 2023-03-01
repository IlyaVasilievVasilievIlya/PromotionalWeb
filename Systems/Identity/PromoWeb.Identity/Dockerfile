#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Identity/PromoWeb.Identity/PromoWeb.Identity.csproj", "Systems/Identity/PromoWeb.Identity/"]
COPY ["Data/PromoWeb.Context.Entities/PromoWeb.Context.Entities.csproj", "Data/PromoWeb.Context.Entities/"]
COPY ["Data/PromoWeb.Context/PromoWeb.Context.csproj", "Data/PromoWeb.Context/"]
COPY ["Shared/PromoWeb.Settings/PromoWeb.Settings.csproj", "Shared/PromoWeb.Settings/"]
COPY ["Shared/PromoWeb.Common/PromoWeb.Common.csproj", "Shared/PromoWeb.Common/"]
RUN dotnet restore "Systems/Identity/PromoWeb.Identity/PromoWeb.Identity.csproj"
COPY . .
WORKDIR "/src/Systems/Identity/PromoWeb.Identity"
RUN dotnet build "PromoWeb.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PromoWeb.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PromoWeb.Identity.dll"]