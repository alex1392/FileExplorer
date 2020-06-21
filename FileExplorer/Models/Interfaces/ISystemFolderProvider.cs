namespace FileExplorer.Models
{
	public interface ISystemFolderProvider
	{
		#region Public Methods

		string GetComputer();

		string GetDesktop();

		string GetDocuments();

		string GetDownloads();

		#endregion Public Methods

		#region Public Methods

		string[] GetLogicalDrives();

		string GetMusic();

		string GetPictures();

		string GetRecent();

		string GetVideos();

		#endregion Public Methods
	}
}