﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
		}

		protected override void OnStop()
		{
		}

		public void RunAsConsole(string[] args)
		{
			OnStart(args);
			Console.WriteLine("Press Enter to exit...");
			Console.ReadLine();
			OnStop();
		}
	}
}
