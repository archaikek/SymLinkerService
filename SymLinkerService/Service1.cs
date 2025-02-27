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
using System.Threading.Tasks;

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
		private void UpdateJunctions()
		{

		}
	}
}
