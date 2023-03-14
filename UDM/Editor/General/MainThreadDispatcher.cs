using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UDM {
	[InitializeOnLoad]
	public class MainThreadDispatcher {

		private static Queue<Action> m_mainThreadActions;

		static MainThreadDispatcher() {
			m_mainThreadActions = new();
			
			EditorApplication.update += Update;
		}

		static void Update() {
			if (m_mainThreadActions.Count == 0) return;
			
			while (m_mainThreadActions.Count > 0) {
				try {
					m_mainThreadActions.Dequeue()?.Invoke();
				}
				catch (Exception e) {
					Debug.LogError(e);
				}
			}
		}

		public static void EnqueueOnMainThread(Action action) {
			m_mainThreadActions.Enqueue(action);
		}
	}
}