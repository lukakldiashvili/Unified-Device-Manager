using System;

namespace UDM.Toolbar {
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class ToolbarButtonAttribute : Attribute {

		public int priority;
		public string icon;
		public string tooltip;
		public string guiStyle;
		public string condition;
		public bool isLeftSide;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="icon"></param>
		/// <param name="priority"></param>
		/// <param name="tooltip"></param>
		/// <param name="guiStyle"></param>
		/// <param name="condition">boolean parameter found in EditorConfiguration's ScriptableObject</param>
		public ToolbarButtonAttribute(string icon, int priority = 0, string tooltip = "", string guiStyle = "AppCommand",
		                              string condition = "", bool isLeftSide = false) {
			this.priority   = priority;
			this.icon       = icon;
			this.tooltip    = tooltip;
			this.guiStyle   = guiStyle;
			this.condition  = condition;
			this.isLeftSide = isLeftSide;
		}
	}	
}