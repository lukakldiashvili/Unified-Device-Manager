using System;
using System.Threading;
using UDM.Build;
using UDM.Handlers.Editor;
using UDM.Helpers;
using UnityEngine;

namespace UDM {
	public partial class UDMPanel {
		[UDMButtonMethod(iconGUID: "55fc8c361d6f54e1e9a360e84ab92ebd", tooltip: "Build, Install and Run")]
		public void Build_Install_Run() {
			// var query = new ADBQuery("");
			//
			// query.SetDevice(m_devicesManager.GetActiveAndroid.SerialOrIP);
			//
			// query.Execute();
			
			// options = DefaultBuildMethods.GetBuildPlayerOptionsInternal(askForBuildLocation, options);
			
			BuildHandler.StartAndroidBuild("_vcignore/Android/Android.apk", RunLastBuild);
		}
		
		[UDMButtonMethod(iconGUID: "b38646c0ef948461c92a9bfcadf83e66", tooltip: "Install and Run")]
		public void InstallLast_Run() {
			RunLastBuild();
		}
		
		[UDMButtonMethod(iconGUID: "6d9a02303d2fc40b89e65164fc6458a8", tooltip: "Run/Restart Installed")]
		public void RunInstalled() {
			StopRunningApp();
			Thread.Sleep(1000);
			StartInstalledAppActivity();
		}
		
		[UDMButtonMethod(iconGUID: "d1ac72284e1d74dd68428d9e16640668", tooltip: "ADB connect Wirelessly")]
		public void ConnectAdbWireless() {
			var    res1 = new ADBQuery("shell ip -f inet addr show wlan0").SetDevice(m_devicesManager).Execute();
			string ip = RegExHelpers.GetIPFromADBMessage(res1);

			Thread.Sleep(500);

			var res2 = new ADBQuery("tcpip 5555").SetDevice(m_devicesManager).Execute();
			
			Thread.Sleep(500);
			
			//connect
			var res3 = new ADBQuery($"connect {ip}:5555").SetDevice(m_devicesManager).Execute();
			
			CloseAllInstances();
		}
		
		// ----

		private void RunLastBuild() {
			StopRunningApp();
			
			if (BuildHandler.TryGetLastBuildPath(out var path)) {
				var result = new ADBQuery($"install {path}").SetDevice(m_devicesManager).Execute();

				StartInstalledAppActivity();
			}
		}

		
		public void StartInstalledAppActivity() {
			var cmd = $"shell am start -n {Application.identifier}/com.unity3d.player.UnityPlayerActivity";
			
			var result = new ADBQuery(cmd).SetDevice(m_devicesManager).Execute();
		}
		
		public void StopRunningApp() {
			var cmd = $"shell am force-stop {Application.identifier}";
			
			var result = new ADBQuery(cmd).SetDevice(m_devicesManager).Execute();
		}

	}
}