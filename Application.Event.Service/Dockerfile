# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY . .
RUN dotnet restore Application.Event.Service/Application.Event.Service.csproj
RUN dotnet publish Application.Event.Service/Application.Event.Service.csproj -c Release -o /app/publish --no-restore

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Application.Event.Service.dll"]