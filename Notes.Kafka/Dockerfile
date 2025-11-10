FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Notes.Kafka/*.csproj ./
RUN dotnet restore

COPY Notes.Kafka/. ./
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app

COPY --from=build /app ./

VOLUME ["/app/Logs"]

ENTRYPOINT ["dotnet", "Notes.Kafka.dll"]
