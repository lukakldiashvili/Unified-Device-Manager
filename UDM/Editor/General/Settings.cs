using UnityEditor;

namespace UDM {
	public class Settings {
		
		private const string ADB_Path_Key = "UDM_ADB_Path";
		
		public static string ADB_Path {
			get => EditorPrefs.GetString(ADB_Path_Key, string.Empty);
			set => EditorPrefs.SetString(ADB_Path_Key, value);
		}
		
	}
}