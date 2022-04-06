#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mechanix.API.Cars/Mechanix.API.Cars.csproj", "Mechanix.API.Cars/"]
COPY ["Mechanix.API.Core/Mechanix.API.Core.csproj", "Mechanix.API.Core/"]
RUN dotnet restore "Mechanix.API.Cars/Mechanix.API.Cars.csproj"
COPY . .
WORKDIR "/src/Mechanix.API.Cars"
RUN dotnet build "Mechanix.API.Cars.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mechanix.API.Cars.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mechanix.API.Cars.dll"]