FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ReadingSummary.Api/ReadingSummary.Api.csproj", "ReadingSummary.Api/"]
COPY ["ReadingList.ServiceDefaults/ReadingList.ServiceDefaults.csproj", "ReadingList.ServiceDefaults/"]
RUN dotnet restore "ReadingSummary.Api/ReadingSummary.Api.csproj"
COPY . .
WORKDIR "/src/ReadingSummary.Api"
RUN dotnet publish "ReadingSummary.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8000
ENV ASPNETCORE_URLS=http://+:8000
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ReadingSummary.Api.dll"]
