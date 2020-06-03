FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Dotmim.Posts.Api/Dotmim.Posts.Api.csproj", "Dotmim.Posts.Api/"]
RUN dotnet restore "Dotmim.Posts.Api/Dotmim.Posts.Api.csproj"
COPY . .
WORKDIR "/src/Dotmim.Posts.Api"
RUN dotnet build "Dotmim.Posts.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Dotmim.Posts.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Dotmim.Posts.Api.dll"]