FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY MicroserviceProject.IdentityServer/*.csproj MicroserviceProject.IdentityServer 
RUN dotnet restore MicroserviceProject.IdentityServer/*.csproj
COPY . .
RUN dotnet publish MicroserviceProject.IdentityServer/MicroserviceProject.IdentityServer.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MicroserviceProject.IdentityServer.dll"]
