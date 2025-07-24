# 1) Etapa base: solo runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 2) Etapa build: SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release

# Copia solo el csproj y restaura dependencias
COPY ["apiReservas/apiReservas.csproj", "apiReservas/"]
RUN dotnet restore "apiReservas/apiReservas.csproj"

# Copia el resto y compila
COPY . .
WORKDIR "/src/apiReservas"
RUN dotnet build "apiReservas.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# 3) Etapa publish: producir artefactos finales
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "apiReservas.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# 4) Etapa final: empaqueta solo lo publicado
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "apiReservas.dll"]


