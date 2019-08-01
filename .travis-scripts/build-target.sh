#! /bin/sh

PROJECT_PATH=$(pwd)/${UNITYCI_PROJECT_NAME}
UNITY_BUILD_DIR=$1
LOG_FILE=$2
BUILD_LABEL=$3
ADDITIONAL_BUILD_ARGS=${@:4}

ERROR_CODE=0

echo "Items in project path ($PROJECT_PATH):"
ls "$PROJECT_PATH"

echo "Building project for ${BUILD_LABEL}..."
mkdir -p ${UNITY_BUILD_DIR}
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile \
  -projectPath "$PROJECT_PATH" \
  ${ADDITIONAL_BUILD_ARGS} \
  -quit \
  | tee "$LOG_FILE"
  
if [[ $? = 0 ]] ; then
  echo "Building $BUILD_LABEL completed successfully."
  ERROR_CODE=0
else
  echo "Building $BUILD_LABEL failed. Exited with $?."
  ERROR_CODE=1
fi

echo 'Build logs:'
cat ${LOG_FILE}

echo "Finishing with code $ERROR_CODE"
exit ${ERROR_CODE}