using System;

namespace UDM {
	[AttributeUsage(AttributeTargets.Method)]
	public class UDMButtonMethod : Attribute {
		public string iconGUID;
		public string tooltip;
		public int priority;
		
		public UDMButtonMethod(string iconGUID = "4169196540306420147", string tooltip = "", int priority = 0) {
			this.iconGUID = iconGUID;
			this.tooltip  = tooltip;
			this.priority = priority;
		}
	}
}