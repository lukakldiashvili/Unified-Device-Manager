using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace UDM.Handlers {
	public class ShellHandler {
		
		public static string ExecuteShellCommand(string command, Action<Process> processOptions = null) {
			string fileName = string.Empty;
			string shellCommand = command;
			
			BasedOnPlatform(() => {
				fileName     = "cmd.exe";
				shellCommand = $"/c \"{shellCommand}\"";
			}, () => {
				fileName     = "/bin/bash";
				shellCommand = $"-c '{shellCommand}'";
			});
			
			return ExecuteCommand(fileName, shellCommand, processOptions);
		}

		public static string ExecuteCommand(string fileName, string command, Action<Process> processOptions = null) {
			Process process = new Process();
			
			process.StartInfo.FileName = fileName;

			process.StartInfo.Arguments = command;

			process.StartInfo.UseShellExecute        = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow         = true;
			process.StartInfo.WindowStyle            = ProcessWindowStyle.Hidden;

			process.StartInfo.RedirectStandardError  = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput  = false;

			var stdOutput = new StringBuilder();
			process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);

			processOptions?.Invoke(process);

			process.Start();
			process.BeginOutputReadLine();

			string error = process.StandardError.ReadToEnd();
			process.WaitForExit();

			string output = stdOutput.ToString();

			if (process.ExitCode != 0) {
				Debug.Log(error + $" (Exit Code: {process.ExitCode.ToString()})");
			}

			return output;
		}

		private static void BasedOnPlatform(Action onWindows, Action onMac) {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				onWindows?.Invoke();
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				onMac?.Invoke();
			}
		}
	}
}