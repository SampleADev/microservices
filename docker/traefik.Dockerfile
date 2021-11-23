FROM traefik

ADD ./traefik.file.toml /etc/traefik/
ADD ./traefik.toml      /etc/traefik/

# CMD traefik --web --docker --file --file.filename=/etc/traefik-cfg/traefik.file.toml --docker.domain=docker.localhost --logLevel=INFO --configFile=/etc/traefik-cfg/traefik.toml