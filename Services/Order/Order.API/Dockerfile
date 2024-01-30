FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY Services/Order/Order.Application/*.csproj Order/Order.Application/
COPY Services/Order/Order.Infrastructure/*.csproj Order/Order.Infrastructure/
COPY Services/Order/Order.Domain/*.csproj Order/Order.Domain/
COPY Services/Order/Order.Domain.Core/*.csproj Order/Order.Domain.Core/



COPY Services/Order/Order.API/*.csproj Services/Order/Order.API/
RUN dotnet restore Services/Order/Order.API/Order.API.csproj
COPY . .
RUN dotnet publish Services/Order/Order.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Order.API.dll"] 