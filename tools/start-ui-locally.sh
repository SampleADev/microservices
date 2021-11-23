DOCKER_IP=$(docker-machine ip)
BASE_PATH=$(cd  $(dirname $0)/.. 2> /dev/null && pwd -P)

cd $BASE_PATH/src/frontend
pwd

env RD_ERP_API_URL=http://${DOCKER_IP}/api RD_ERP_LOGIN_URL=http://auth.rd-erp.io/connect/authorize npx webpack-dev-server --mode=development --profile --colors