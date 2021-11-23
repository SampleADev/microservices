# RD Erp

## Platform

- PostgreSQL
- .NET Core + Linq2DB + asp.net core
- Typescript + React + Webpack
- 3-rd party identity server
- Docker + Docker compose

- Microservice architecture

## Folder structure

### `frontend`

Contains react app uses the microservices

### `libs`

Backend libraries shared between services
May contain a business logic, interfaces, communication libs etc.

### `services`

Contains microservices running up by the docker compose.
`frontend` is one of the microservices, but have it's own folder at top level.

### `up`

Configuration files for running application with docker-compose

### `db`

Contains a set of migrations updates everything to the latest db version.

### Migration rules

- Migrations should be fool-proof.
- Always try to check if migration could be applied.
  Migration tools rely on internal versioning mechanism
  but that is not convenient for developing and testing your migrations.
- It would be great if migration could be executed as regular scrip
