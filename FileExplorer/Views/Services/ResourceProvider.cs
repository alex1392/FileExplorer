using FileExplorer.Models;

namespace FileExplorer.Views.Services
{
	public class ResourceProvider : IResourceProvider
	{
		#region Public Methods

		public object FindResource(string resourceKey)
		{
			return App.Current.FindResource(resourceKey);
		}

		public object TryFindResource(string resourceKey)
		{
			return App.Current.TryFindResource(resourceKey);
		}

		#endregion Public Methods
	}
}