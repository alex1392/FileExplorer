using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows.Media;

namespace FileExplorer.ViewModels
{
	public class ListFolderItemViewModel : ListItemViewModel
	{
		#region Public Constructors

		public ListFolderItemViewModel(IServiceProvider serviceProvider, IDispatcherService dispatcherService) : base(serviceProvider, dispatcherService)
		{
		}

		#endregion Public Constructors

		#region Protected Methods

		protected override ImageSource GetIcon()
		{
			ImageSource source = null;
			dispatcherService.Invoke(() =>
			{
				// image source must be created at the main thread
				source = App.Current.TryFindResource("FolderIcon") as ImageSource;
			});
			return source;
		}

		protected override void SetItem()
		{
			Item = serviceProvider.GetService<ListFolderItem>();
			Item.Path = Path;
		}

		#endregion Protected Methods
	}
}