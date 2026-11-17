FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ReadingList.Api/ReadingList.Api.csproj", "ReadingList.Api/"]
COPY ["ReadingList.ServiceDefaults/ReadingList.ServiceDefaults.csproj", "ReadingList.ServiceDefaults/"]
RUN dotnet restore "ReadingList.Api/ReadingList.Api.csproj"
COPY . .
WORKDIR "/src/ReadingList.Api"
RUN dotnet publish "ReadingList.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8000
ENV ASPNETCORE_URLS=http://+:8000
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ReadingList.Api.dll"]
