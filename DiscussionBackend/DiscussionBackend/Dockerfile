FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./DiscussionBackend/DiscussionBackend.csproj", "DiscussionBackend/"]
RUN dotnet restore "DiscussionBackend/DiscussionBackend.csproj"
COPY . .
WORKDIR "/src/DiscussionBackend"
RUN dotnet build "DiscussionBackend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DiscussionBackend.csproj" -c $BUILD_CONFIGURATION -o /app/publish 
#/p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscussionBackend.dll"]
