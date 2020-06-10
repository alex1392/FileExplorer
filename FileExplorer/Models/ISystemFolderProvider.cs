namespace FileExplorer.Models {
	internal interface ISystemFolderProvider {
		string[] GetLogicalDrives();
		string GetRecentFolder();
	}
}
