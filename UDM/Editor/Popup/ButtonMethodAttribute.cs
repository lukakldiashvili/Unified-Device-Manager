using System;

namespace UDM {
	[AttributeUsage(AttributeTargets.Method)]
	public class ButtonMethodAttribute : Attribute {
		public string iconGUID;
		public string tooltip;
		public string condition;
		public int priority;
		
		public ButtonMethodAttribute(string iconGUID = "4169196540306420147", string tooltip = "", int priority = 0, string condition = "") {
			this.iconGUID  = iconGUID;
			this.tooltip   = tooltip;
			this.priority  = priority;
			this.condition = condition;
		}
	}
}