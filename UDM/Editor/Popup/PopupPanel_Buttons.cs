using System.Threading;
using UDM.Android;
using UDM.Build;
using UDM.Helpers;
using UnityEngine;

namespace UDM {
	public partial class PopupPanel {
		[ButtonMethod(iconGUID: Constants.Icons.BUILD_INSTALL_RUN_GUID, tooltip: "Build, Install and Run")]
		public void Build_Install_Run() {
			BuildHandler.StartAndroidBuild("_vcignore/Android/Android.apk", RunLastBuild);
		}
		
		[ButtonMethod(iconGUID: Constants.Icons.INSTALL_LAST_RUN_GUID, tooltip: "Install and Run")]
		public void InstallLast_Run() {
			RunLastBuild();
		}
		
		[ButtonMethod(iconGUID: Constants.Icons.RUN_INSTALLED_GUID, tooltip: "Run/Restart Installed")]
		public void RunInstalled() {
			StopRunningApp();
			Thread.Sleep(1000);
			StartInstalledAppActivity();
		}
		
		[ButtonMethod(iconGUID: Constants.Icons.ADB_WIRELESS_GUID, tooltip: "ADB connect Wireless", condition: nameof(ConnectAdbWireless_Condition))]
		public void ConnectAdbWireless() {
			var    msg = new ADBQuery("shell ip -f inet addr show wlan0").SetDevice(m_deviceManager).Execute();
			string ip = RegExHelpers.GetIPFromADBMessage(msg);

			Thread.Sleep(500);

			new ADBQuery("tcpip 5555").SetDevice(m_deviceManager).Execute();
			
			Thread.Sleep(500);
			
			//connect
			new ADBQuery($"connect {ip}:5555").SetDevice(m_deviceManager).Execute();
			
			CloseAllInstances();
		}
		
		// ----
		// conditions
		
		private bool ConnectAdbWireless_Condition() {
			// return m_devicesManager.GetAndroids.All(device => !device.IsWireless);
			return false;
		}
		
		
		// ----

		private void RunLastBuild() {
			StopRunningApp();
			
			if (BuildHandler.TryGetLastBuildPath(out var path)) {
				new ADBQuery($"install {path}").SetDevice(m_deviceManager).Execute();

				StartInstalledAppActivity();
			}
		}

		
		public void StartInstalledAppActivity() {
			var cmd = $"shell am start -n {Application.identifier}/com.unity3d.player.UnityPlayerActivity";

			new ADBQuery(cmd).SetDevice(m_deviceManager).Execute();
		}
		
		public void StopRunningApp() {
			var cmd = $"shell am force-stop {Application.identifier}";

			new ADBQuery(cmd).SetDevice(m_deviceManager).Execute();
		}

	}
}