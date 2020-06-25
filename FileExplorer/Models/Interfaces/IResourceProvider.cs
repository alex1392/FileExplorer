namespace FileExplorer.Models
{
	public interface IResourceProvider
	{
		#region Public Methods

		object FindResource(string resourceKey);

		object TryFindResource(string resourceKey);

		#endregion Public Methods
	}
}