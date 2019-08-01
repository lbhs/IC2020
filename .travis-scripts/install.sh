#! /bin/sh

# Download Unity3D installer into the container
#  The below link will need to change depending on the version, this one is for 5.5.1
#  Refer to https://unity3d.com/get-unity/download/archive and find the link pointed to by Mac "Unity Editor"
UNITY_VERSION=2018-3.14f1
EDITOR_LINK=https://download.unity3d.com/download_unity/d0e9f15437b1/MacEditorInstaller/Unity.pkg?_ga=2.190232171.1336623465.1564675467-2040511779.1564675467
WEBGL_LINK=https://download.unity3d.com/download_unity/d0e9f15437b1/MacEditorTargetInstaller/UnitySetup-WebGL-Support-for-Editor-2018.3.14f1.pkg?_ga=2.198032367.1336623465.1564675467-2040511779.1564675467

echo "Downloading Unity $UNITY_VERSION pkg:"
curl --retry 5 -o Unity.pkg ${EDITOR_LINK}
if [[ $? -ne 0 ]]; then { echo "Download failed"; exit $?; }; fi

# In Unity 5 they split up build platform support into modules which are installed separately
# By default, only Mac OSX support is included in the original editor package; Windows, Linux, iOS, Android, and others are separate
# In this example we download Windows support. Refer to http://unity.grimdork.net/ to see what form the URLs should take
echo "Downloading Unity $UNITY_VERSION webGL Support pkg:"
curl --retry 5 -o Unity_webgl.pkg ${WEBGL_LINK}
if [[ $? -ne 0 ]]; then { echo "Download failed"; exit $?; }; fi

# Run installer(s)
echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
echo 'Installing Unity_win.pkg'
sudo installer -dumplog -package Unity_webgl.pkg -target /