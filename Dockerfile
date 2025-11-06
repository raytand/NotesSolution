FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY Notes.Api/*.csproj Notes.Api/
COPY Notes.Application/*.csproj Notes.Application/
COPY Notes.Domain/*.csproj Notes.Domain/
COPY Notes.Infrastructure/*.csproj Notes.Infrastructure/
COPY Notes.Tests/*.csproj Notes.Tests/

RUN dotnet restore NotesSolution.sln

COPY . .

WORKDIR /src/Notes.Api
RUN dotnet publish -c Release -o /app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Notes.Api.dll"]
