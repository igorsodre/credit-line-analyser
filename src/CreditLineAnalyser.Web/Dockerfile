FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CreditLineAnalyser.Web/CreditLineAnalyser.Web.csproj", "CreditLineAnalyser.Web/"]
RUN dotnet restore "CreditLineAnalyser.Web/CreditLineAnalyser.Web.csproj"
COPY ["src/CreditLineAnalyser.Web", "CreditLineAnalyser.Web"]
WORKDIR "/src/CreditLineAnalyser.Web"
RUN dotnet build "CreditLineAnalyser.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CreditLineAnalyser.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CreditLineAnalyser.Web.dll"]
