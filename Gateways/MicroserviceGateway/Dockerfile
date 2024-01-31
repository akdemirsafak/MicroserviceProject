FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

COPY Gateways/MicroserviceGateway/*.csproj Gateways/MicroserviceGateway/
RUN dotnet restore Gateways/MicroserviceGateway/*.csproj
COPY . .
RUN dotnet publish Gateways/MicroserviceGateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MicroserviceGateway.dll"] 