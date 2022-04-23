#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tuya.PruebaTecnica.OrderService/Tuya.PruebaTecnica.OrderService.csproj", "Tuya.PruebaTecnica.OrderService/"]
RUN dotnet restore "Tuya.PruebaTecnica.OrderService/Tuya.PruebaTecnica.OrderService.csproj"
COPY . .
WORKDIR "/src/Tuya.PruebaTecnica.OrderService"
RUN dotnet build "Tuya.PruebaTecnica.OrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tuya.PruebaTecnica.OrderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tuya.PruebaTecnica.OrderService.dll"]