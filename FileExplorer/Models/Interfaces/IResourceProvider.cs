namespace FileExplorer.Models
{
	public interface IResourceProvider
	{
		object FindResource(string resourceKey);
		object TryFindResource(string resourceKey);
	}
}