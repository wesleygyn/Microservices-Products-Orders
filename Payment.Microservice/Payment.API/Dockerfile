FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Payment.API/Payment.API.csproj", "Payment.API/"]
COPY ["Payment.Application/Payment.Application.csproj", "Payment.Application/"]
COPY ["Payment.Domain/Payment.Domain.csproj", "Payment.Domain/"]
COPY ["Payment.Infrastructure/Payment.Infrastructure.csproj", "Payment.Infrastructure/"]

RUN dotnet restore "Payment.API/Payment.API.csproj"

COPY . .

WORKDIR "/src/Payment.API"
RUN dotnet build "Payment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.API.dll"]