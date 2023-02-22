#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorkerServiceTest/WorkerServiceTest.csproj", "WorkerServiceTest/"]
RUN dotnet restore "WorkerServiceTest/WorkerServiceTest.csproj"
COPY . .
WORKDIR "/src/WorkerServiceTest"
RUN dotnet build "WorkerServiceTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerServiceTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerServiceTest.dll"]