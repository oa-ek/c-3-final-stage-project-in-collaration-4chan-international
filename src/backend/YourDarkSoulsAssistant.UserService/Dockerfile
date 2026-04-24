FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
RUN apk add --no-cache krb5-libs
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/backend/YourDarkSoulsAssistant.UserService/YourDarkSoulsAssistant.UserService.csproj", "src/backend/YourDarkSoulsAssistant.UserService/"]
COPY ["src/backend/YourDarkSoulsAssistant.ServiceDefaults/YourDarkSoulsAssistant.ServiceDefaults.csproj", "src/backend/YourDarkSoulsAssistant.ServiceDefaults/"]

RUN dotnet restore "src/backend/YourDarkSoulsAssistant.UserService/YourDarkSoulsAssistant.UserService.csproj"

COPY . .

WORKDIR "/src/src/backend/YourDarkSoulsAssistant.UserService"

RUN dotnet build "./YourDarkSoulsAssistant.UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YourDarkSoulsAssistant.UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourDarkSoulsAssistant.UserService.dll"]
