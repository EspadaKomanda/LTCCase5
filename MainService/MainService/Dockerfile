FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
EXPOSE 7293
ENV ASPNETCORE_URLS=http://+:7293
COPY . .
WORKDIR /src
COPY ["MainService/MainService.csproj", "MainService/"]
RUN dotnet restore "MainService/MainService.csproj"
COPY . .
WORKDIR "/src/MainService"
RUN dotnet publish "MainService.csproj" -c Release -o /app/publish /p:UseAppHost=false 
WORKDIR /app/publish
COPY . . 
ENTRYPOINT ["dotnet", "MainService.dll"]
