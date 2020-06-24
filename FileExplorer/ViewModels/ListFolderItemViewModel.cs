using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileExplorer.ViewModels
{
	public class ListFolderItemViewModel : ListItemViewModel
	{
		private readonly IResourceProvider resourceProvider;

		#region Public Constructors

		public ListFolderItemViewModel(IServiceProvider serviceProvider, IDispatcherService dispatcherService, IResourceProvider resourceProvider) : base(serviceProvider, dispatcherService)
		{
			this.resourceProvider = resourceProvider;
		}

		#endregion Public Constructors

		#region Protected Methods

		protected override async Task GetIconAsync()
		{
			await Task.Run(async () =>
			{
				//await Task.Delay(1000).ConfigureAwait(false);

				// Doesn't need to call this line on main thread, since the resource is already created in xaml, which was on the main thread. Here it simply retrieves that resource.
				Icon = resourceProvider.TryFindResource("FolderIcon") as ImageSource;
			}).ConfigureAwait(false);
		}

		[Obsolete]
		protected override ImageSource GetIcon()
		{
			Task.Delay(1000).Wait();
			return resourceProvider.TryFindResource("FolderIcon") as ImageSource; 
		}

		protected override void SetItem()
		{
			Item = serviceProvider.GetService<ListFolderItem>();
			Item.Path = Path;
		}

		#endregion Protected Methods
	}
}