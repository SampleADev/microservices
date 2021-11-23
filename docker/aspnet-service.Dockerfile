FROM microsoft/dotnet:2.1-sdk as build

ARG PROJECT_FOLDER="src/services"
ARG PROJECT_NAME

RUN echo ${PROJECT_FOLDER}/${PROJECT_NAME}

ADD src/libs /app/libs
ADD ${PROJECT_FOLDER}/${PROJECT_NAME} /app/services/current

WORKDIR /app/services/current
RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c Release -o out



FROM microsoft/dotnet:2.1.1-aspnetcore-runtime AS runtime
ARG PROJECT_NAME
COPY --from=build /app/services/current/out ./app
ADD db /app/db
WORKDIR /app