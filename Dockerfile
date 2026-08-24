# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project file first
COPY ["JobApplicationTracker.csproj", "./"]

# Restore dependencies
RUN dotnet restore "JobApplicationTracker.csproj"

# Copy the rest of the project
COPY . .

# Build and publish
RUN dotnet publish "JobApplicationTracker.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Railway will provide PORT
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "JobApplicationTracker.dll"]