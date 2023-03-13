using System.Linq;
using System.Reflection;
using UDM.Helpers;
using UDM.Toolbar;
using UnityEditor;
using UnityEngine;

namespace UDM {
	public partial class PopupPanel : EditorWindow {
		private static readonly Vector2 s_windowSize = new Vector2(250, 150);

		private const int MAX_BUTTON_WIDTH = 42;
		private const int MAX_BUTTON_HEIGHT = 36;


		private GUILayoutOption[] m_buttonOptions;

		private Button_Data[] m_buttons;

		private DevicesManager m_devicesManager;

		[ToolbarButton(Constants.Icons.MAIN_ICON_GUID, isLeftSide: true)]
		public static void OpenUDMPanel_Button() {
			CloseAllInstances();

			PopupPanel window = ScriptableObject.CreateInstance<PopupPanel>();

			float yPos = Event.current.mousePosition.y + (s_windowSize.y / 2 - 10);

			window.position = new Rect(Event.current.mousePosition.x, yPos, s_windowSize.x, s_windowSize.y);
			window.ShowPopup();

			window.Init();
			window.Focus();
		}

		public static void CloseAllInstances() {
			var windows = (PopupPanel[])Resources.FindObjectsOfTypeAll(typeof(PopupPanel));

			foreach (var window in windows) {
				window.Close();
			}
		}

		void OnGUI() {
			if (!CheckBeforeGUI()) return;

			EditorGUILayout.BeginHorizontal();

			GUI_DrawMainButtons();

			EditorGUILayout.EndHorizontal();

			m_devicesManager.GUI_DrawDropdown();
		}

		private void OnDestroy() {
			m_devicesManager.SaveState();
		}

		public void Init() {
			m_devicesManager = DevicesManager.GetOrCreate();
			
			LoadButtons();

			m_buttonOptions = new[] {
				GUILayout.MaxWidth(s_windowSize.x / m_buttons.Length), GUILayout.MaxHeight(MAX_BUTTON_HEIGHT)
			};
		}

		void LoadButtons() {
			var allMethods =
				typeof(PopupPanel).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			var buttonMethods = allMethods
			                .Where(method => method.GetCustomAttributes().Any(attr => attr is ButtonMethodAttribute));

			buttonMethods = buttonMethods.OrderBy(method => method.GetCustomAttribute<ButtonMethodAttribute>().priority);

			m_buttons = buttonMethods.Select(method => {
				var methodAttr = method.GetCustomAttribute<ButtonMethodAttribute>();
				var conditionMethodInfo = allMethods.FirstOrDefault(m => m.Name == methodAttr.condition);

				bool enabled = conditionMethodInfo == null || (bool) conditionMethodInfo.Invoke(this, null);
				
				return new Button_Data() {
					methodInfo = method,
					enabled    = enabled,
					icon       = Utilities.LoadTextureWithGUID(methodAttr.iconGUID),
					tooltip    = methodAttr.tooltip
				};
			}).ToArray();
		}

		private void OnLostFocus() {
			this.Close();
		}

		bool CheckBeforeGUI() {
			if (m_devicesManager.GetAndroids.Length != 0) {
				return true;
			}

			EditorGUILayout.LabelField("No devices found.");
			EditorGUILayout.LabelField("Please connect your device and try again.");

			if (GUILayout.Button("Close")) {
				Close();
			}
			
			return false;
		}

		void GUI_DrawMainButtons() {
			foreach (var buttonData in m_buttons) {
				EditorGUI.BeginDisabledGroup(!buttonData.enabled);
				
				if (GUILayout.Button(new GUIContent(image: buttonData.icon, tooltip: buttonData.tooltip), m_buttonOptions)) {
					buttonData.methodInfo?.Invoke(this, null);
				}
				
				EditorGUI.EndDisabledGroup();
			}
		}
	}
}