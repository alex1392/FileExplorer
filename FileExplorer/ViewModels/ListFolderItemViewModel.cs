using FileExplorer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileExplorer.ViewModels
{
	public class ListFolderItemViewModel : ListItemViewModel
	{

		public ListFolderItemViewModel(IServiceProvider serviceProvider, IDispatcherService dispatcherService) : base(serviceProvider, dispatcherService)
		{
		}
		protected override ImageSource GetIcon()
		{
			ImageSource source = null;
			dispatcherService.Invoke(() =>
			{
				// image source must be created at the main thread
				source = new BitmapImage(new Uri("/Resources/Folder.ico", UriKind.Relative));
			});
			return source;
		}

		protected override void SetItem()
		{
			Item = serviceProvider.GetService<ListFolderItem>();
			Item.Path = Path;
		}
	}
}
