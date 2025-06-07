FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GamedevQuest/GamedevQuest.csproj", "GamedevQuest/"]
RUN dotnet restore "./GamedevQuest/GamedevQuest.csproj"
COPY . .
WORKDIR "/src/GamedevQuest"
RUN dotnet build "./GamedevQuest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GamedevQuest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GamedevQuest.dll"]