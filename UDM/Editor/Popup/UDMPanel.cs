using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UDM.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace UDM {
	public partial class UDMPanel : EditorWindow {
		private static readonly Vector2 s_windowSize = new Vector2(250, 150);

		private const string MAIN_ICON_GUID = "4a2c0e3bd7d7a4b379ca986b22cc48e9";

		private const int MAX_BUTTON_WIDTH = 42;
		private const int MAX_BUTTON_HEIGHT = 36;

		private Texture2D m_mainIconTexture;

		private GUILayoutOption[] m_buttonOptions;

		private UDMButton_Data[] m_buttons;

		private DevicesManager m_devicesManager;

		[ToolbarButton("d164c837878644cd79b7bf331acf6381", isLeftSide: true)]
		public static void OpenUDMPanel_Button() {
			CloseAllInstances();

			UDMPanel window = ScriptableObject.CreateInstance<UDMPanel>();

			float yPos = Event.current.mousePosition.y + (s_windowSize.y / 2 - 10);

			window.position = new Rect(Event.current.mousePosition.x, yPos, s_windowSize.x, s_windowSize.y);
			window.ShowPopup();

			window.Init();
			window.Focus();
		}

		public static void CloseAllInstances() {
			var windows = (UDMPanel[])Resources.FindObjectsOfTypeAll(typeof(UDMPanel));

			foreach (var window in windows) {
				window.Close();
			}
		}

		void OnGUI() {
			EditorGUILayout.BeginHorizontal();

			GUI_DrawMainButtons();

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space(10);

			m_devicesManager.GUI_DrawDropdown();
		}

		private void OnDestroy() {
			m_devicesManager.SaveState();
		}

		public void Init() {
			m_devicesManager = DevicesManager.GetOrCreate();
			
			LoadAssets();
			LoadButtons();

			m_buttonOptions = new[] {
				GUILayout.MaxWidth(s_windowSize.x / m_buttons.Length), GUILayout.MaxHeight(MAX_BUTTON_HEIGHT)
			};
		}

		void LoadButtons() {
			var buttonMethods = typeof(UDMPanel).GetMethods()
			                .Where(method => method.GetCustomAttributes().Any(attr => attr is UDMButtonMethod));

			buttonMethods = buttonMethods.OrderBy(method => method.GetCustomAttribute<UDMButtonMethod>().priority);

			m_buttons = buttonMethods.Select(method => {
				var methodAttr = method.GetCustomAttribute<UDMButtonMethod>();
				
				return new UDMButton_Data() {
					methodInfo = method,
					icon       = UDMHelpers.LoadTextureWithGUID(methodAttr.iconGUID),
					tooltip    = methodAttr.tooltip
				};
			}).ToArray();
		}

		void LoadAssets() {
			m_mainIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(MAIN_ICON_GUID));
		}
		

		private void OnLostFocus() {
			this.Close();
		}

		void GUI_DrawMainButtons() {
			foreach (var buttonData in m_buttons) {
				if (GUILayout.Button(new GUIContent(image: buttonData.icon, tooltip: buttonData.tooltip), m_buttonOptions)) {
					buttonData.methodInfo?.Invoke(this, null);
				}
			}
		}
	}
}