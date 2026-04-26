FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY . .
RUN dotnet restore Application.Subscription.Service/Application.Subscription.Service.csproj
RUN dotnet publish Application.Subscription.Service/Application.Subscription.Service.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Application.Subscription.Service.dll"]