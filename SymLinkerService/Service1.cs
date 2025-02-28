using Monitor.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SymLinkerService
{
	public partial class Service1 : ServiceBase
	{
		public Service1()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			Console.WriteLine("Starting...");

			int interval = 1800; // 30 minute timer by default
			try
			{
				interval = Int32.Parse(args[0]);
			}
			catch { }
			Console.WriteLine("Started!");

			var timer = new System.Timers.Timer(TimeSpan.FromSeconds(interval).TotalMilliseconds);
			timer.AutoReset = true;
			timer.Elapsed += UpdateJunctions;
			timer.Start();
			
		}

		protected override void OnStop()
		{
			Console.WriteLine("Stopping...");
			Console.WriteLine("Stopped!");
		}

		public void RunAsConsole(string[] args)
		{
			OnStart(args);
			Console.WriteLine("Press Enter to exit...");
			Console.ReadLine();
			OnStop();
		}

		private void UpdateJunctions(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("Tick!");
			List<LinkedDirectoriesHelper.LinkedDirectoryInfo> dirs;
			try
			{
				dirs = LinkedDirectoriesHelper.GetLinkedDirectories();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to read the config file (most likely the XML structure is incorrect.");
				return;
			}
			foreach (var dir in dirs)
			{
				var srcDirs = Directory.GetDirectories(dir.source).Where(d => !dir.ignores.Contains(d));
				foreach (var srcDir in srcDirs)
				{
					var name = Path.GetFileName(srcDir);
					try
					{
						Console.WriteLine($"Trying {dir.target}\\{name}");
						JunctionPoint.Create($"{dir.target}\\{name}", srcDir, false);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Exception at {srcDir}. Details:\n{ex}\n\n");
					}
				}
			}
		}
	}
}
