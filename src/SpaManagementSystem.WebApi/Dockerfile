FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:8081;http://+:8080
ENV ASPNETCORE_HTTPS_PORT=8081

ENV ASPNETCORE_Kestrel__Certificates__Default__Password="maslo#2!XX"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/certs/aspnetapp.pfx"

ENV SENDGRID_API_KEY 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
ENV SENDGRID_SENDER_EMAIL "zml18x@gmail.com"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SpaManagementSystem.WebApi/SpaManagementSystem.WebApi.csproj", "src/SpaManagementSystem.WebApi/"]
COPY ["src/SpaManagementSystem.Domain/SpaManagementSystem.Domain.csproj", "src/SpaManagementSystem.Domain/"]
COPY ["src/SpaManagementSystem.Application/SpaManagementSystem.Application.csproj", "src/SpaManagementSystem.Application/"]
COPY ["src/SpaManagementSystem.Infrastructure/SpaManagementSystem.Infrastructure.csproj", "src/SpaManagementSystem.Infrastructure/"]
RUN dotnet restore "src/SpaManagementSystem.WebApi/SpaManagementSystem.WebApi.csproj"
COPY . .
WORKDIR "/src/src/SpaManagementSystem.WebApi"
RUN dotnet build "SpaManagementSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SpaManagementSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./certs /app/certs
ENTRYPOINT ["dotnet", "SpaManagementSystem.WebApi.dll"]
