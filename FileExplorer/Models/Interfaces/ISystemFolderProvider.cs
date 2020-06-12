namespace FileExplorer.Models {

	public interface ISystemFolderProvider {

		#region Public Methods

		string[] GetLogicalDrives();

		string GetRecentFolder();

		#endregion Public Methods
	}
}