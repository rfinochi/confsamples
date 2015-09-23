FROM microsoft/aspnet

COPY . /app
WORKDIR /app
RUN ["dnu", "restore"]

EXPOSE 5003
ENTRYPOINT ["dnx", "-p", "project.json", "kestrel"]