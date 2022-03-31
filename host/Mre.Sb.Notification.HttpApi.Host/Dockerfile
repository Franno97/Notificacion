#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["host/Mre.Sb.Notification.HttpApi.Host/Mre.Sb.Notification.HttpApi.Host.csproj", "host/Mre.Sb.Notification.HttpApi.Host/"]
COPY ["host/Mre.Sb.Notification.Host.Shared/Mre.Sb.Notification.Host.Shared.csproj", "host/Mre.Sb.Notification.Host.Shared/"]
COPY ["src/Mre.Sb.Notification.Application/Mre.Sb.Notification.Application.csproj", "src/Mre.Sb.Notification.Application/"]
COPY ["src/Mre.Sb.Notification.Application.Contracts/Mre.Sb.Notification.Application.Contracts.csproj", "src/Mre.Sb.Notification.Application.Contracts/"]
COPY ["src/Mre.Sb.Notification.Domain.Shared/Mre.Sb.Notification.Domain.Shared.csproj", "src/Mre.Sb.Notification.Domain.Shared/"]
COPY ["src/Mre.Sb.Notification.Domain/Mre.Sb.Notification.Domain.csproj", "src/Mre.Sb.Notification.Domain/"]
COPY ["src/Mre.Sb.Notification.HttpApi/Mre.Sb.Notification.HttpApi.csproj", "src/Mre.Sb.Notification.HttpApi/"]
COPY ["src/Mre.Sb.Notification.EntityFrameworkCore/Mre.Sb.Notification.EntityFrameworkCore.csproj", "src/Mre.Sb.Notification.EntityFrameworkCore/"]
RUN dotnet restore "host/Mre.Sb.Notification.HttpApi.Host/Mre.Sb.Notification.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/host/Mre.Sb.Notification.HttpApi.Host"
RUN dotnet build "Mre.Sb.Notification.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mre.Sb.Notification.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mre.Sb.Notification.HttpApi.Host.dll"]
