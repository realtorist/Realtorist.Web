#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
ARG GITHUB_USER
ARG NUGET_PAT
WORKDIR /src
COPY ["Realtorist.Web/Realtorist.Web.csproj", "Realtorist.Web/"]
COPY ["nuget.config", "nuget.config"]
RUN dotnet restore "Realtorist.Web/Realtorist.Web.csproj"
COPY . .
WORKDIR "/src/Realtorist.Web"
RUN dotnet build "Realtorist.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Realtorist.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Extensions", "Extensions"]
ENTRYPOINT ["dotnet", "Realtorist.Web.dll"]