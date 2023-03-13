using UDM.Handlers.Editor;
using UnityEngine;

namespace UDM {
	public partial class UDMPanel {
		[UDMButtonMethod(iconGUID: "55fc8c361d6f54e1e9a360e84ab92ebd", tooltip: "Build, Install and Run")]
		public void Build_Install_Run() {
			var devices = ADBHandler.GetActiveDevices();

		}
		
		[UDMButtonMethod(iconGUID: "b38646c0ef948461c92a9bfcadf83e66", tooltip: "Install last build and Run")]
		public void InstallLast_Run() {
			// Do something
		}
		
		[UDMButtonMethod(iconGUID: "6d9a02303d2fc40b89e65164fc6458a8", tooltip: "Run already installed")]
		public void RunInstalled() {
			ADBHandler.StartInstalledAppActivity();
		}
		
		[UDMButtonMethod(iconGUID: "d1ac72284e1d74dd68428d9e16640668", tooltip: "ADB connect Wirelessly")]
		public void ConnectAdbWireless() {
			// Do something
		}
	}
}