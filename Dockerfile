FROM microsoft/dotnet:2.0-sdk

COPY . /app

WORKDIR /app/RoutingAndSpectrumAllocation

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]