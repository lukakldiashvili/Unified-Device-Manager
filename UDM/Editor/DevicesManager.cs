using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UDM.Handlers.Editor;
using UDM.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace UDM {
	[Serializable]
	public class DevicesManager {
		
		private const string SESSION_STATE_KEY = "UDM.DevicesManager.State";
		
		private ADBDevice[] m_androids;
		
		public ADBDevice[] GetAndroids => m_androids;

		public ADBDevice GetSelectedAndroid => m_androids[m_selectedAndroidIndex];
		
		[SerializeField] public int m_selectedAndroidIndex;
		
		private string[] m_androidDeviceOptions;

		public static DevicesManager GetOrCreate() {
			DevicesManager manager = null;
			
			var prevState = SessionState.GetString(SESSION_STATE_KEY, String.Empty);

			if (!String.IsNullOrEmpty(prevState)) {
				manager = UDMHelpers.ByteArrayToObject<DevicesManager>(Convert.FromBase64String(prevState));
			}
			else {
				manager = new DevicesManager();
			}
			
			manager.Init();

			return manager;
		}

		private void Init() {
			Load();

			m_androidDeviceOptions =
				m_androids.Select(device => $"{device.Model} ({device.SerialOrIP})").ToArray();
		}

		private void Load() {
			m_androids = ADBHandler.GetActiveDevices();
		}

		// public void GUI_DrawSelectedAndroid() {
		// 	EditorGUILayout.LabelField($"Selected: {GetSelectedAndroid.Model} ({GetSelectedAndroid.SerialOrIP})");
		// }
		
		public void GUI_DrawDropdown() {
			EditorGUI.BeginChangeCheck();
				
			EditorGUILayout.LabelField($"Selected Device:");
			m_selectedAndroidIndex = EditorGUILayout.Popup(String.Empty, m_selectedAndroidIndex, m_androidDeviceOptions);
			
			if (EditorGUI.EndChangeCheck())
			{
				// Debug.Log(_options[_selected]);
			}
		}
		
		public void SaveState() {
			var bytes = UDMHelpers.ObjectToByteArray(this);

			var str = Convert.ToBase64String(bytes);
			
			SessionState.SetString(SESSION_STATE_KEY, str);
		}
	}
}