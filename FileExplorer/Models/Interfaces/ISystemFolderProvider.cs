namespace FileExplorer.Models {
	public interface ISystemFolderProvider {
		string[] GetLogicalDrives();
		string GetRecentFolder();
	}
}
