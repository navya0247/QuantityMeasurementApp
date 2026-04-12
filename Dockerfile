FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY QuantityMeasurementApp.sln .
COPY QuantityMeasurementApp.API/ QuantityMeasurementApp.API/
COPY QuantityMeasurementApp.BusinessLayer/ QuantityMeasurementApp.BusinessLayer/
COPY QuantityMeasurementApp.ModelLayer/ QuantityMeasurementApp.ModelLayer/
COPY QuantityMeasurementApp.RepoLayer/ QuantityMeasurementApp.RepoLayer/

RUN dotnet restore QuantityMeasurementApp.API/QuantityMeasurementApp.API.csproj
RUN dotnet publish QuantityMeasurementApp.API/QuantityMeasurementApp.API.csproj \
    -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "QuantityMeasurementApp.API.dll"]