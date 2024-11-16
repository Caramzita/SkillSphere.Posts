FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
ENV ASPNETCORE_URLS=http://+:8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN dotnet nuget add source http://host.docker.internal:5000/v3/index.json -n baget

COPY ["src/SkillSphere.Posts.API/SkillSphere.Posts.API.csproj", "src/SkillSphere.Posts.API/"]
COPY ["src/SkillSphere.Posts.Contracts/SkillSphere.Posts.Contracts.csproj", "src/SkillSphere.Posts.Contracts/"]
COPY ["src/SkillSphere.Posts.Core/SkillSphere.Posts.Core.csproj", "src/SkillSphere.Posts.Core/"]
COPY ["src/SkillSphere.Posts.DataAccess/SkillSphere.Posts.DataAccess.csproj", "src/SkillSphere.Posts.DataAccess/"]
COPY ["src/SkillSphere.Posts.UseCases/SkillSphere.Posts.UseCases.csproj", "src/SkillSphere.Posts.UseCases/"]
RUN dotnet restore "./src/SkillSphere.Posts.API/SkillSphere.Posts.API.csproj"

COPY . .
WORKDIR "/src/src/SkillSphere.Posts.API"
RUN dotnet build "./SkillSphere.Posts.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SkillSphere.Posts.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillSphere.Posts.API.dll"]