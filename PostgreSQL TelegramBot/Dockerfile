FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PostgreSQL TelegramBot.csproj", "PostgreSQL TelegramBot/"]
RUN dotnet restore "PostgreSQL TelegramBot/PostgreSQL TelegramBot.csproj"
WORKDIR "/src/PostgreSQL TelegramBot"
COPY . .
RUN dotnet build "PostgreSQL TelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostgreSQL TelegramBot/PostgreSQL TelegramBot.csproj" -c Release -o /out
COPY "App.config" /out

FROM base AS final
WORKDIR /out
ENTRYPOINT ["dotnet", "PostgreSQL TelegramBot.dll"]