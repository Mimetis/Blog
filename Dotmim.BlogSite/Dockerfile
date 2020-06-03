FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Dotmim.BlogSite/Dotmim.BlogSite.csproj", "Dotmim.BlogSite/"]
RUN dotnet restore "Dotmim.BlogSite/Dotmim.BlogSite.csproj"
COPY . .
WORKDIR "/src/Dotmim.BlogSite"
RUN dotnet build "Dotmim.BlogSite.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Dotmim.BlogSite.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Dotmim.BlogSite.dll"]
