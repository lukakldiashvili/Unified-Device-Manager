using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
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

		public ADBDevice GetActiveAndroid => m_androids[m_activeAndroidIndex];
		
		[SerializeField] private int m_activeAndroidIndex;
		
		private string[] m_androidDeviceOptions;

		[SerializeField] private string m_stateHash;

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
			
			var newStateHash = ComputeStateHash();
			
			if (!m_stateHash.Equals(newStateHash)) {
				Reset();
			}

			m_stateHash = newStateHash;
		}

		private string ComputeStateHash() {
			return Convert.ToBase64String(
				new SHA1Managed().ComputeHash(Encoding.Unicode.GetBytes(String.Join(' ', m_androidDeviceOptions))));
		}

		private void Load() {
			m_androids = ADBHandler.GetActiveDevices();
		}

		private void Reset() {
			m_activeAndroidIndex = m_androidDeviceOptions.Length - 1;
		}

		// public void GUI_DrawSelectedAndroid() {
		// 	EditorGUILayout.LabelField($"Selected: {GetSelectedAndroid.Model} ({GetSelectedAndroid.SerialOrIP})");
		// }
		
		public void GUI_DrawDropdown() {
			EditorGUI.BeginChangeCheck();
				
			EditorGUILayout.LabelField($"Selected Device:");
			m_activeAndroidIndex = EditorGUILayout.Popup(String.Empty, m_activeAndroidIndex, m_androidDeviceOptions);
			
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