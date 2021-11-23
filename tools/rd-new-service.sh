!#/bin/bash

echo "Usage: ./rd-new-service <service-name>"
echo "<service-name> Pascal-case name of the service"
echo "Omit RdErp prefix and .Service, .Core etc. suffixes"

if [[ $# -eq 0 ]]; then
    echo "Service name is required";
    exit 1;
fi

SERVICE_NAME=$1
SERVICE_NAME_LOWER=$(echo "$SERVICE_NAME" | tr '[:upper:]' '[:lower:]')
BASE_PATH=$(cd  $(dirname $0)/.. 2> /dev/null && pwd -P)

echo $SERVICE_NAME
echo $SERVICE_NAME_LOWER
echo $BASE_PATH

sh "$BASE_PATH/tools/dotnet-new.sh" classlib "libs/$SERVICE_NAME_LOWER/RdErp.$SERVICE_NAME.Contract"
sh "$BASE_PATH/tools/dotnet-new.sh" classlib "libs/$SERVICE_NAME_LOWER/RdErp.$SERVICE_NAME.Core" "libs/cross-cutting"
sh "$BASE_PATH/tools/dotnet-new.sh" classlib "libs/$SERVICE_NAME_LOWER/RdErp.$SERVICE_NAME.DataAccess" "libs/cross-cutting"

# absolute path to project library folder.
# Usage classlib_folder $libname
# $libname is name of library without prefix (like Contract, Core, etc.)
function classlib_folder() {
    echo "$BASE_PATH/src/libs/$SERVICE_NAME_LOWER/RdErp.$SERVICE_NAME.${1}"
}

# absolute path to project library csproj file.
# Usage classlib_project $libname
# $libname is name of library without prefix (like Contract, Core, etc.)
function classlib_project() {
    local FOLDER=$( classlib_folder ${1} )
    echo "$FOLDER/RdErp.$SERVICE_NAME.${1}.csproj"
}

dotnet add $(classlib_project Contract) package --version 2.2.0 Linq2Db

dotnet add $(classlib_project Core) reference $(classlib_project Contract)
dotnet add $(classlib_project Core) reference $(classlib_project DataAccess)
dotnet add $(classlib_project Core) package --version 2.1.0 Microsoft.Extensions.DependencyInjection
dotnet add $(classlib_project Core) package FluentValidation


dotnet add $(classlib_project DataAccess) reference $(classlib_project Contract)
dotnet add $(classlib_project DataAccess) package --version 2.1.0 Microsoft.Extensions.DependencyInjection
dotnet add $(classlib_project DataAccess) package --version 2.2.0 Linq2Db
dotnet add $(classlib_project DataAccess) package npgsql

# Create service
sh "$BASE_PATH/tools/dotnet-new.sh" web "services/RdErp.$SERVICE_NAME.Service"

WEB_PROJECT="$BASE_PATH/src/services/RdErp.$SERVICE_NAME.Service/RdErp.$SERVICE_NAME.Service.csproj"

dotnet add "$WEB_PROJECT" reference $(classlib_project Contract)
dotnet add "$WEB_PROJECT" reference $(classlib_project Core)
dotnet add "$WEB_PROJECT" reference $(classlib_project DataAccess)
