#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Account_Service/Account_Service.csproj", "Account_Service/"]
RUN dotnet restore "Account_Service/Account_Service.csproj"
COPY . .
WORKDIR "/src/Account_Service"
RUN dotnet build "Account_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Account_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Account_Service.dll"]