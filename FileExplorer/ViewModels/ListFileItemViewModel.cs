using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Drawing = System.Drawing;

namespace FileExplorer.ViewModels
{
	public class ListFileItemViewModel : ListItemViewModel
	{
		#region Public Constructors

		public ListFileItemViewModel(IServiceProvider serviceProvider, IDispatcherService dispatcherService) : base(serviceProvider, dispatcherService)
		{
		}

		#endregion Public Constructors

		#region Protected Methods

		protected override void SetItem()
		{
			Item = serviceProvider.GetService<ListFileItem>();
			Item.Path = Path;
		}

		protected override ImageSource GetIcon()
		{
			var icon = Drawing::Icon.ExtractAssociatedIcon(Path);
			ImageSource source = null;
			dispatcherService.Invoke(() =>
			{
				// image source must be created at the main thread
				source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			});
			return source;
		}

		#endregion Protected Methods
	}
}