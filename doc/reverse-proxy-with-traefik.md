# Better microservices with reverse proxy

## Why

When using microservice architecture, there are two possible ways of consuming it.
One is to know exact microservice structure, hosts, ports, etc.
and address each service by its own unique URL.

Second is to create a facade which hides actual microservices and exposes single endpoint for accessing all application functionality without actually knowing which microservice serves particular request.

This way has some advantages:

- Pass-throught authentication and request securing.
  Authentication token could be used for all microservices.
  Strictly said this could be achieved without having single host for all microservices,
  but token origin validation should be turned off.
  Also it is possible to use cookies throught all microservices.

- Nice deploy-time configurable versioning
  by running both older and newer version of services under different paths.

- Simpler cross-service communication using hosts + proxy rules.
  This allows to create "internal" host with different routing rules
  and use it for calling another service from inside some service.

## Solutions overview

We will use `Docker` and `docker-compose` as container orchestrator.
Basically the widely-used reverse-proxy is `nginx`,
but we will consider using alternative solution named `traefik`
