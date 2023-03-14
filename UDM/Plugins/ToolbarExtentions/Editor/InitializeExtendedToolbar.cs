using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UDM.Helpers;
using UDM.Toolbar.Plugin;
using UnityEditor;
using UnityEngine;

namespace UDM.Toolbar {
	[InitializeOnLoad]
	public static class InitializeExtendedToolbar {
		static InitializeExtendedToolbar() {
			EditorApplication.delayCall += UpdateToolbars;
		}

		public static void UpdateToolbars() {
			ToolbarExtender.RightToolbarGUI.Clear();
			ToolbarExtender.LeftToolbarGUI.Clear();


			var toolbarButtonMethods = Utilities
			                           .GetMethodsWithAttribute(new[] {Assembly.GetExecutingAssembly()},
				                           typeof(ToolbarButtonAttribute),
				                           methodsFlags: BindingFlags.Static | BindingFlags.Public |
				                                         BindingFlags.NonPublic).Where(info => info.IsStatic).ToList();

			IEnumerable<(MethodInfo info, ToolbarButtonAttribute attribute)> btnAndAttrs =
				toolbarButtonMethods.Select(info =>
					(info, (ToolbarButtonAttribute) info.GetCustomAttribute(typeof(ToolbarButtonAttribute))));

			btnAndAttrs = btnAndAttrs.OrderBy(a => a.attribute.priority);

			foreach (var btnAndAttr in btnAndAttrs) {
				var tex = Utilities.LoadTextureWithGUID(btnAndAttr.attribute.icon);
				// var        tex       = EditorGUIUtility.IconContent(btnAndAttr.attribute.icon).image;
				GUIContent tbContent = new GUIContent(null, tex, btnAndAttr.attribute.tooltip);

				bool allowAdd = true;

				if (allowAdd) {
					List<Action> side = btnAndAttr.attribute.isLeftSide
						? ToolbarExtender.LeftToolbarGUI
						: ToolbarExtender.RightToolbarGUI;

					side.Add(() => OnToolbarGUI(tbContent, btnAndAttr.attribute.guiStyle,
						() => btnAndAttr.info.Invoke(null, null)));
				}
			}

			ToolbarCallback.RepaintLastRoot();
		}

		static void OnToolbarGUI(GUIContent toolbarContent, string guiStyle, Action onPress) {
			EditorGUIUtility.SetIconSize(new Vector2(16, 16));
			
			if (GUILayout.Button(toolbarContent, guiStyle)) {
				onPress?.Invoke();
			}
		}
	}
}