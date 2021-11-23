echo "Creates dotnet project and adds it to solution"
echo "Usage dotnet-new <type> <path-to-project/relative-to/src/dir> <dir/to/add/refs/from>"

PROJECT_TYPE=$1
PROJECT_PATH=$2
ARG_REF_DIR=$3

BASE_PATH=$(cd  $(dirname $0)/.. 2> /dev/null && pwd -P)

SRC_PATH="$BASE_PATH/src"
REF_DIR="$BASE_PATH/src/$ARG_REF_DIR"
PROJECT_NAME="$(basename ${PROJECT_PATH})"

PROJECT_FOLDER="$SRC_PATH/$PROJECT_PATH"
PROJECT_FULL_PATH="$PROJECT_FOLDER/$PROJECT_NAME.csproj"
SLN_FULL_PATH="$SRC_PATH/RdErp.sln"


echo $PROJECT_FOLDER
echo $SLN_FULL_PATH
echo $PROJECT_FULL_PATH
echo $PROJECT_NAME

dotnet new ${PROJECT_TYPE} --force --name "$PROJECT_NAME" --output "$PROJECT_FOLDER"
dotnet sln ${SLN_FULL_PATH} add ${PROJECT_FULL_PATH}


function addRefs() {
    FROM_DIR=$1

    echo "Add references from $FROM_DIR"

    for REF in $FROM_DIR/**/*.csproj; do
        dotnet add "$PROJECT_FULL_PATH" reference "$REF"
    done
}

echo "Add common references"
addRefs "$SRC_PATH/libs/cross-cutting"

if [[ -n $ARG_REF_DIR ]]; then
    echo "Add references from $REF_DIR"
    addRefs $REF_DIR
fi

