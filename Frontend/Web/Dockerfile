FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY Frontend/Web/*.csproj Frontend/Web/ 
RUN dotnet restore Frontend/Web/*.csproj
COPY . .
RUN dotnet publish Frontend/Web/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Web.dll"]