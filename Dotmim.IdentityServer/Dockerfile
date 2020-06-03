FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Dotmim.IdentityServer/Dotmim.IdentityServer.csproj", "Dotmim.IdentityServer/"]
RUN dotnet restore "Dotmim.IdentityServer/Dotmim.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Dotmim.IdentityServer"
RUN dotnet build "Dotmim.IdentityServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Dotmim.IdentityServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Dotmim.IdentityServer.dll"]