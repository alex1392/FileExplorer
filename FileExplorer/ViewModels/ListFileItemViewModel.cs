using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;
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

		protected override async Task GetIconAsync()
		{
			await Task.Run(async () =>
			{
				await Task.Delay(0).ConfigureAwait(false);
				return Drawing::Icon.ExtractAssociatedIcon(Path);
			}).ContinueWith(task =>
			{
				// image source must be created at main thread
				// however, sometimes the exception may not occurs, perhaps the imaging method uses certain caching method?
				Icon = Imaging.CreateBitmapSourceFromHIcon(task.Result.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			},
			// force the continuation task executing on the main thread
			// this is only valid when the task is originated on the main thread
			TaskScheduler.FromCurrentSynchronizationContext());
		}

		[Obsolete]
		protected override ImageSource GetIcon()
		{
			Task.Delay(1000).Wait();
			var icon = Drawing::Icon.ExtractAssociatedIcon(Path);
			return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}

		#endregion Protected Methods
	}
}