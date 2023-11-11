FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
EXPOSE 7023
EXPOSE 5178
ENV ASPNETCORE_URLS=http://+:7023;http://+:5178
COPY . .
WORKDIR /src
COPY ["ConfigService/ConfigService.csproj", "ConfigService/"]
RUN dotnet restore "ConfigService/ConfigService.csproj"
COPY . .
WORKDIR "/src/ConfigService"
RUN dotnet publish "ConfigService.csproj" -c Release -o /app/publish /p:UseAppHost=false 
WORKDIR /app/publish
COPY . .
ENTRYPOINT ["dotnet", "ConfigService.dll"]
