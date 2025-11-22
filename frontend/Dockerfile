#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore "WEB.APP.csproj"

RUN dotnet build "./WEB.APP.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "./WEB.APP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


# Temporarily switch to root user to set permissions
USER root
RUN mkdir -p /app/wwwroot/assets/resources && chmod -R 777 /app/wwwroot/assets/resources
RUN apt update && apt install -y passwd
RUN apt update && apt install -y nano
RUN echo 'root:P@ssw0rd123' | chpasswd

# Switch back to app user for security
USER app

ENTRYPOINT ["dotnet", "WEB.APP.dll"]