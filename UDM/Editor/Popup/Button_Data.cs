using System.Reflection;
using UnityEngine;

namespace UDM {
	public struct Button_Data {
		public MethodInfo methodInfo;
		public bool enabled;
		public Texture2D icon;
		public string tooltip;
	}
}