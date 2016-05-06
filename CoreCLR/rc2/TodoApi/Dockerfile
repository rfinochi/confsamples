FROM microsoft/dotnet-preview:latest
EXPOSE 5004/tcp

COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]

RUN dotnet build --configuration release

ENV ASPNETCORE_ENVIRONMENT production 

ENTRYPOINT dotnet run --configuration release --server.urls http://*:5004
