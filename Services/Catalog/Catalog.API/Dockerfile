FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY Services/Catalog/Catalog.API/*.csproj Services/Catalog/Catalog.API/
RUN dotnet restore Services/Catalog/Catalog.API/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/Catalog.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]