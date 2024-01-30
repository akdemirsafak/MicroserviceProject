FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY Services/FakePayment/FakePayment.API/*.csproj Services/FakePayment/FakePayment.API/
RUN dotnet restore Services/FakePayment/FakePayment.API/FakePayment.API.csproj
COPY . .
RUN dotnet publish Services/FakePayment/FakePayment.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "FakePayment.API.dll"] 