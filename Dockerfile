#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
#WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS svcbuild
WORKDIR /src
COPY . .
RUN dotnet publish -c Release Dapr.Server.sln
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/AccountService/Host/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "Host.dll"]