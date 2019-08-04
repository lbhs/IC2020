using UnityEditor;
using UnityEngine;

namespace Tools
{
    namespace Editor
    {
        /// <summary>
        /// Helper behavior to make it easier to consistently compile the app
        /// </summary>
        public class BuildWebGL : MonoBehaviour
        {
            [MenuItem("IC2020/Build WebGL")]
            static void BuildWebGLTarget()
            {
                Debug.Log("Building the WebGL Target");
                ExecuteBuild(BuildTarget.WebGL);
            }

            [MenuItem("IC2020/Build and Run WebGL")]
            static void BuildAndRunWebGLTarget()
            {
                Debug.Log("Building and running the WebGL Target");
                ExecuteBuild(BuildTarget.WebGL, BuildOptions.AutoRunPlayer);
            }

            [MenuItem("IC2020/Build Win64")]
            static void BuildWindowsTarget()
            {
                Debug.Log("Building the Win64 Target");
                ExecuteBuild(BuildTarget.StandaloneWindows64);
            }

            [MenuItem("IC2020/Build macOS")]
            static void BuildMacOSTarget()
            {
                Debug.Log("Building the macOS Target");
                ExecuteBuild(BuildTarget.StandaloneOSX);
            }

            /// <summary>
            /// Uses the Unity BuildPipeline to generate binaries
            /// </summary>
            /// <param name="target">The <see cref="BuildTarget">BuildTarget</see> to build to</param>
            /// <param name="options">Additional options (such as <see cref="BuildOptions.AutoRunPlayer">AutoRunPlayer</see>) for the build pipeline.</param>
            private static void ExecuteBuild(BuildTarget target, BuildOptions options = BuildOptions.None)
            {
                string pathSuffix = target.ToString();
                // Place all your scenes here
                string[] scenes =
                {
                    "Assets/Scenes/base.unity"
                };

                string pathToDeploy = "Build/" + pathSuffix + "/";
                BuildPipeline.BuildPlayer(scenes, pathToDeploy, target, options);
            }
        }
    }
}