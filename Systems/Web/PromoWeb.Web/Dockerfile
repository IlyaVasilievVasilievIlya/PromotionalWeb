FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Web/PromoWeb.Web/PromoWeb.Web.csproj", "Systems/Web/PromoWeb.Web/PromoWeb.Web.csproj"]
RUN dotnet restore "Systems/Web/PromoWeb.Web/PromoWeb.Web.csproj"
COPY . .
WORKDIR "/src/Systems/Web/PromoWeb.Web"
RUN dotnet build "PromoWeb.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PromoWeb.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine
WORKDIR /var/www/web

COPY --from=publish /app/publish/wwwroot .
COPY Systems/Web/PromoWeb.Web/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80