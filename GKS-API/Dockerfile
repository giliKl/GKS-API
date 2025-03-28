# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["GKS-API/GKS-API.csproj", "GKS-API/"]
COPY ["GKD.Data/GKD.Data.csproj", "GKD.Data/"]
COPY ["GKS.Core/GKS.Core.csproj", "GKS.Core/"]
COPY ["GKS.Service/GKS.Service.csproj", "GKS.Service/"]



# Run dotnet restore
RUN dotnet restore "./GKS-API/GKS-API.csproj"

# Copy all source files
COPY . .

# Set working directory and build
WORKDIR "/src/GKS-API"
RUN dotnet build "./GKS-API.csproj" -c %BUILD_CONFIGURATION% -o /app/build
