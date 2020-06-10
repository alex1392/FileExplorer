using System;
using System.IO;

namespace FileExplorer.Models {
	class SystemFolderProvider : ISystemFolderProvider {

		public string GetRecentFolder()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Recent);
		}
		public string[] GetLogicalDrives()
		{
			return Directory.GetLogicalDrives();
		}
	}
}
