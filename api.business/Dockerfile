#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src


COPY . .
RUN dotnet restore "./Services/MachineDashboardAPI/MachineDashboardAPI.csproj"
WORKDIR "/src/Services/MachineDashboardAPI"
RUN dotnet build "./MachineDashboardAPI.csproj" -c Release -o /app/build
RUN dotnet publish "./MachineDashboardAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Set Timezone to Asia/Bangkok
ENV TZ=Asia/Bangkok
RUN apt-get update && apt-get install -y tzdata && \
    ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
    echo $TZ > /etc/timezone && \
    dpkg-reconfigure -f noninteractive tzdata && \
    apt-get clean

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MachineDashboardAPI.dll"]