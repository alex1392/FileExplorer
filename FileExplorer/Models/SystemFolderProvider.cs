using System;
using System.IO;

namespace FileExplorer.Models {

	internal class SystemFolderProvider : ISystemFolderProvider {

		#region Public Methods

		public string[] GetLogicalDrives()
		{
			return Directory.GetLogicalDrives();
		}

		public string GetRecentFolder()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Recent);
		}

		#endregion Public Methods
	}
}