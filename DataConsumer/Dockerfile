#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DataConsumer/DataConsumer.csproj", "DataConsumer/"]
COPY ["RozetkaDto/RozetkaDto.csproj", "RozetkaDto/"]
RUN dotnet restore "DataConsumer/DataConsumer.csproj"
COPY . .
WORKDIR "/src/DataConsumer"
RUN dotnet build "DataConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataConsumer.dll"]