FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2019 AS base
WORKDIR /app
EXPOSE 5925:5925

RUN powershell Invoke-WebRequest -Uri https://artifacts.elastic.co/downloads/beats/filebeat/filebeat-8.5.1-windows-x86_64.zip -OutFile filebeat.zip; \
    Expand-Archive -Path filebeat.zip -DestinationPath C:\filebeat; \
    Remove-Item -Force filebeat.zip

COPY filebeat.yml C:/filebeat/filebeat.yml

FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2019 AS build
WORKDIR /src
COPY ["API_Secure.csproj", "."]
RUN dotnet restore "/API_Secure.csproj"
COPY . . 
WORKDIR "/src/."
RUN dotnet build "API_Secure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_Secure.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

CMD powershell -Command \
    Start-Process -NoNewWindow -FilePath 'C:\\filebeat\\filebeat.exe'; \
    Start-Sleep -Seconds 2; \
    dotnet API_Secure.dll
