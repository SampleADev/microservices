docker-compose stop postgresql

docker-compose rm -v -f -s postgresql

docker-compose up --no-deps -d postgresql

sleep 20

docker-compose stop postgresql

docker-compose up --no-deps -d postgresql
