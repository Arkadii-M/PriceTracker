#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
## Install curl. Need to check if server is running
RUN apt-get update && apt-get install -y \
	curl

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GraphQLServer/GraphQLServer.csproj", "GraphQLServer/"]
RUN dotnet restore "GraphQLServer/GraphQLServer.csproj"
COPY . .
WORKDIR "/src/GraphQLServer"
RUN dotnet build "GraphQLServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQLServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQLServer.dll"]