using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;

namespace UDM.Build {
	public class BuildHandler {

		private const string LAST_BUILD_PATH = "BuildHandler_" + nameof(LastBuildPath);
	
		private static string LastBuildPath {
			get => EditorPrefs.GetString(LAST_BUILD_PATH);

			set => EditorPrefs.SetString(LAST_BUILD_PATH, value);
		}

		[PostProcessBuild(1)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
			LastBuildPath = pathToBuiltProject;
		}

		public static bool TryGetLastBuildPath(out string path) {
			path = "\"" + LastBuildPath + "\"";
			return !String.IsNullOrEmpty(LastBuildPath);
		}

		// location: "_vcignore/Android/Android.apk"
		public static void StartAndroidBuild(string location, Action onSuccess = null, BuildOptions options = BuildOptions.None) {
			BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
				scenes           = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
				locationPathName = location,
				target           = BuildTarget.Android,
				options          = options,
			};
			
			var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

			if (report.summary.result == BuildResult.Succeeded) {
				onSuccess?.Invoke();
			}
		}
		
	}
}