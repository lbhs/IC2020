#! /bin/sh

# NOTE the command args below make the assumption that your Unity project folder is
#  a subdirectory of the repo root directory, e.g. for this repo "unity-ci-test" 
#  the project folder is "UnityProject". If this is not true then adjust the 
#  -projectPath argument to point to the right location.

## Run the editor unit tests
function runUnitTests() {
    echo "Running editor unit tests for ${UNITYCI_PROJECT_NAME}"
    /Applications/Unity/Unity.app/Contents/MacOS/Unity \
        -batchmode \
        -nographics \
        -silent-crashes \
        -logFile $(pwd)/unity.log \
        -projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
        -runEditorTests \
        -editorTestsResultFile $(pwd)/test.xml \
        -quit
    
    rc0=$?
    echo "Unit test logs"
    cat $(pwd)/test.xml
    # exit if tests failed
    if [[ ${rc0} -ne 0 ]]; then { echo "Failed unit tests"; exit ${rc0}; }; fi
}

### Uncomment after there's unit tests
# runUnitTests

## Make the builds
function buildWindows() {
    echo "Attempting build of ${UNITYCI_PROJECT_NAME} for Windows"
    /Applications/Unity/Unity.app/Contents/MacOS/Unity \
        -batchmode \
        -nographics \
        -silent-crashes \
        -logFile $(pwd)/unity.log \
        -projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
        -buildWindowsPlayer "$(pwd)/Build/windows/${UNITYCI_PROJECT_NAME}.exe" \
        -quit
    
    winResult=$?
    echo "Build logs (Windows)"
    cat $(pwd)/unity.log
}

function buildOSX() {
    echo "Attempting build of ${UNITYCI_PROJECT_NAME} for OSX"
    /Applications/Unity/Unity.app/Contents/MacOS/Unity \
        -batchmode \
        -nographics \
        -silent-crashes \
        -logFile $(pwd)/unity.log \
        -projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
        -buildOSXUniversalPlayer "$(pwd)/Build/osx/${UNITYCI_PROJECT_NAME}.app" \
        -quit
    
    osxResult=$?
    echo "Build logs (OSX)"
    cat $(pwd)/unity.log
}

function buildWebGL() {
    echo "Attempting build of ${UNITYCI_PROJECT_NAME} for WebGL"
    /Applications/Unity/Unity.app/Contents/MacOS/Unity \
        -batchmode \
        -nographics \
        -silent-crashes \
        -logFile $(pwd)/unity.log \
        -projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
        -buildTarget WebGL \
        -quit
    
    webglResult=$?
    echo "Build logs (WebGL)"
    cat $(pwd)/unity.log
}

buildOSX
buildWindows
buildWebGL

exit $(winResult|osxResult|webglResult)