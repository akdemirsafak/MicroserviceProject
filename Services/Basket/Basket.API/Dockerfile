FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY Services/Basket/Basket.API/*.csproj Services/Basket/Basket.API/
RUN dotnet restore Services/Basket/Basket.API/Basket.API.csproj
COPY . .
RUN dotnet publish Services/Basket/Basket.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Basket.API.dll"]