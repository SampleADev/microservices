echo "Usage docker-compose-restart-service service-name"

SVC=$1

echo "Restarting service $SVC"

docker-compose stop "$SVC"
docker-compose up --build --no-deps -d $SVC