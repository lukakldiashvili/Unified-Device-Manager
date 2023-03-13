using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Android;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UDM.Handlers.Editor {
	public static class ADBHandler {
		
		public static string GetAdbPath => AndroidExternalToolsSettings.sdkRootPath + "/platform-tools/adb";


		public static void StartInstalledAppActivity() {
			ShellHandler.ExecuteCommand(GetAdbPath, GetActivityStartCommand(Application.identifier));
		}

		public static void StopRunningApp() {
			ShellHandler.ExecuteCommand(GetAdbPath, GetActivityStopCommand(Application.identifier));
		}

		private static string GetActivityStartCommand(string identifier) {
			return $"shell am start -n {identifier}/com.unity3d.player.UnityPlayerActivity";
		}
		
		private static string GetActivityStopCommand(string identifier) {
			return $"shell am force-stop {identifier}";
		}

		public static ADBDevice[] GetActiveDevices() {
			List<ADBDevice> devices = new();
			
			var msg = ShellHandler.ExecuteCommand(GetAdbPath, "devices -l");

			var lines = msg.Split("\n");

			foreach (var line in lines) {
				if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
					continue;
				
				if (line.Contains("List of")) continue;
				
				devices.Add(ADBDevice.ParseAndCreate(line));
			}

			return devices.ToArray();
		}
	}
}