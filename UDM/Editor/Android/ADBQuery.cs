using System;
using System.Collections.Generic;

namespace UDM.Android {
	public class ADBQuery {

		private string m_command;
		private List<string> m_suffixArguments;

		private string m_device;

		public ADBQuery(string command) {
			m_command = command;
		}

		public ADBQuery SetDevice(DeviceManager deviceManager) {
			m_device = deviceManager.GetActiveAndroid.SerialOrIP;
			
			return this;
		}

		
		public ADBQuery SetDevice(string deviceIpOrSerial) {
			m_device = deviceIpOrSerial;
			
			return this;
		}

		public ADBQuery SetSuffixArgument(string argument) {
			m_suffixArguments.Add(argument);

			return this;
		}

		public string Execute() {
			return ADBHandler.ExecuteADBQuery(this);
		}

		public override string ToString() {
			var deviceSpecifier = String.IsNullOrEmpty(m_device) ? String.Empty : $"-s {m_device}";

			return $"{deviceSpecifier} {m_command}";
		}
	}
}