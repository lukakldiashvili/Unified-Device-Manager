using System;
using UDM.Helpers;

namespace UDM.Android {
	[Serializable]
	public class ADBDevice {
		public string SerialOrIP { get; set; }
		public string Model { get; set; }
		
		public bool IsWireless => SerialOrIP.Contains(":");

		public static ADBDevice ParseAndCreate(string line) {
			var model = RegExHelpers.GetArgument(line, "model");
			var serial = RegExHelpers.GetFirstWord(line);

			return new ADBDevice() {
				Model      = model,
				SerialOrIP = serial
			};
		}
	}
}