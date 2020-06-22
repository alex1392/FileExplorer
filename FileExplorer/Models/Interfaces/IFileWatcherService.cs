namespace FileExplorer.Models
{
	public interface IFileWatcherService
	{
		string Path { get; set; }

		void Start();
	}
}