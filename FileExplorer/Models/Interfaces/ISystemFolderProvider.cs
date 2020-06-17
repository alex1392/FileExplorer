namespace FileExplorer.Models
{
	public interface ISystemFolderProvider
	{
		string GetComputer();
		string GetDesktop();
		string GetDocuments();
		string GetDownloads();
		#region Public Methods

		string[] GetLogicalDrives();
		string GetMusic();
		string GetPictures();
		string GetRecent();
		string GetVideos();

		#endregion Public Methods
	}
}