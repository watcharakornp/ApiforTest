#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 9947
ENV ASPNETCORE_URLS=http://+:9947
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["APi.csproj", "."]
RUN dotnet restore "./APi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "APi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APi.csproj" -c Release -o /app/publish 

# build testrunner image
FROM build AS testrunner
WORKDIR /source
COPY . .
ENTRYPOINT ["dotnet", "test", "--logger:trx"]



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APi.dll"]