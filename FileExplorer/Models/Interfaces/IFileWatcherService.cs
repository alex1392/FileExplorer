namespace FileExplorer.Models
{
	public interface IFileWatcherService
	{
		#region Public Properties

		string Path { get; set; }

		#endregion Public Properties

		#region Public Methods

		void Start();

		#endregion Public Methods
	}
}