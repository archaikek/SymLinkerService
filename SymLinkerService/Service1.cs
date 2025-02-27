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

			int interval = Int32.Parse(ConfigurationManager.AppSettings["Interval"]);

			Console.WriteLine("Started!");

			var timer = new System.Timers.Timer(TimeSpan.FromMinutes(interval).TotalMilliseconds);
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
			var dirs = LinkedDirectoriesHelper.GetLinkedDirectories();
			foreach (var dir in dirs)
			{
				var srcDirs = Directory.GetDirectories(dir.source).Where(d => !dir.ignores.Contains(d));
				foreach (var srcDir in srcDirs)
				{
					Console.WriteLine($"Processing: {srcDir}");
					var name = Path.GetFileName(srcDir);
					try
					{
						Console.WriteLine($"Trying {dir.target}\\{name}");
						JunctionPoint.Create($"{dir.target}\\{name}", srcDir, false);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Exception at {srcDir}. Details:\n{ex}");
					}
				}
			}
		}
	}
}
