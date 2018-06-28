FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 55940
EXPOSE 44322

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY CityInfo.API/CityInfo.API.csproj CityInfo.API/
RUN dotnet restore CityInfo.API/CityInfo.API.csproj
COPY . .
WORKDIR /src/CityInfo.API
RUN dotnet build CityInfo.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CityInfo.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CityInfo.API.dll"]
