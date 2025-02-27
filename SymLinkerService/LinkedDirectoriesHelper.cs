using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SymLinkerService
{
	internal static class LinkedDirectoriesHelper
	{
		public static readonly string configPath = Directory.GetCurrentDirectory() + "../../../LinkedDirectories.xml";
		public static List<LinkedDirectoryInfo> GetLinkedDirectories()
		{
			List<LinkedDirectoryInfo> results = new List<LinkedDirectoryInfo>();

			XmlDocument document = new XmlDocument();
			document.TryLoad(configPath, 100, 20);
			XmlNode root = document.DocumentElement.SelectSingleNode("/Directories");

			foreach (XmlNode directory in root.ChildNodes) // /Directories/Directory
			{
				var newdir = new LinkedDirectoryInfo();
				var srcAttr = directory.Attributes["source"];
				var trgAttr = directory.Attributes["target"];

				if (srcAttr is null || trgAttr is null)
				{
					continue;
				}
				else
				{
					newdir.source = srcAttr.Value;
					newdir.target = trgAttr.Value;
					foreach (XmlNode ignore in directory.ChildNodes) // /Directories/Directory/Ignore
					{
						var nameAttr = ignore.Attributes["name"];
						if (nameAttr is null)
						{
							continue;
						}
						else
						{
							newdir.ignores.Add(nameAttr.Value);
						}
					}

				}

				results.Add(newdir);
			}

			return results;
		}

		private static void TryLoad(this XmlDocument document, string path, int maxAttempts, int timeoutBetweenAttempts)
		{
			int attempt = 0;
			while (true)
			{
				Console.WriteLine($"Attempt {attempt}");
				try
				{
					document.Load(path);
				}
				catch (Exception ex)
				{
					if (attempt < maxAttempts)
					{
						++attempt;
						Thread.Sleep(timeoutBetweenAttempts);
						continue;
					}
					else
					{
						Console.WriteLine("Failed to access config file.");
						throw ex;
					}
				}
				break;
			}
		}
		public class LinkedDirectoryInfo
		{
			public string source, target;
			public List<string> ignores = new List<string>();

			public LinkedDirectoryInfo() { }
			public LinkedDirectoryInfo(string source, string target)
			{
				this.source = source;
				this.target = target;
			}
			public override string ToString()
			{
				return $"{source}/* -> {target}/*\n" +
					$"\t excluding:\n{GetIgnoreList()}";
			}
			private string GetIgnoreList()
			{
				string result = "";
				foreach (string ignore in ignores)
				{
					result += $"\t- {ignore}\n";
				}

				return result;	
			}
		}
	}
}
