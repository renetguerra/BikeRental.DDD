# Compilation
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln ./
COPY BikeRental.DDD.API/*.csproj ./BikeRental.DDD.API/
COPY BikeRental.DDD.Application/*.csproj ./BikeRental.DDD.Application/
COPY BikeRental.DDD.Domain/*.csproj ./BikeRental.DDD.Domain/
COPY BikeRental.DDD.Infrastructure/*.csproj ./BikeRental.DDD.Infrastructure/
COPY BikeRental.Tests/*.csproj ./BikeRental.Tests/
RUN dotnet restore

# COPY . .
COPY BikeRental.DDD.API/. ./BikeRental.DDD.API/
COPY BikeRental.DDD.Application/. ./BikeRental.DDD.Application/
COPY BikeRental.DDD.Domain/. ./BikeRental.DDD.Domain/
COPY BikeRental.DDD.Infrastructure/. ./BikeRental.DDD.Infrastructure/
COPY BikeRental.Tests/. ./BikeRental.Tests/

WORKDIR /app/BikeRental.DDD.API
RUN dotnet publish -c Release -o /app/out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Install mssql-tools
RUN apt-get update && \
    apt-get install -y curl gnupg apt-transport-https && \
    curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > /etc/apt/trusted.gpg.d/microsoft.gpg && \
    echo "deb [arch=amd64 signed-by=/etc/apt/trusted.gpg.d/microsoft.gpg] https://packages.microsoft.com/ubuntu/20.04/prod focal main" > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev

# Install dockerize
RUN apt-get update && apt-get install -y curl && \
    curl -sSL https://github.com/jwilder/dockerize/releases/download/v0.6.1/dockerize-linux-amd64-v0.6.1.tar.gz | tar -xzv -C /usr/local/bin


ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
EXPOSE 5000
EXPOSE 5001
EXPOSE 80

CMD ["dockerize", "-wait", "tcp://bikerental-db:1433", "-timeout", "300s", "dotnet", "BikeRental.DDD.API.dll"]
