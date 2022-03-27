FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BYOC.Data/BYOC.Data.csproj", "BYOC.Data/"]
COPY ["BYOC.Server/BYOC.Server.csproj", "BYOC.Server/"]
RUN dotnet restore "BYOC.Server/BYOC.Server.csproj"
COPY . .
WORKDIR "/src/BYOC.Server"
RUN dotnet build "BYOC.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BYOC.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BYOC.Server.dll"]
