using FileExplorer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace FileExplorer.ViewModels {

	public class MainWindowViewModel : INotifyPropertyChanged {

		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly IFolderNavigationService folderNavigationService;
		private readonly IServiceProvider serviceProvider;
		private readonly ISystemFolderProvider systemFolderProvider;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public ObservableCollection<TreeFolderItem> TreeItems { get; set; } = new ObservableCollection<TreeFolderItem>();

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// for xaml designer
		/// </summary>
		public MainWindowViewModel()
		{
		}

		public MainWindowViewModel(IFileProvider fileProvider, ISystemFolderProvider systemFolderProvider, IFolderNavigationService folderNavigationService, IServiceProvider serviceProvider)
		{
			this.fileProvider = fileProvider;
			this.systemFolderProvider = systemFolderProvider;
			this.folderNavigationService = folderNavigationService;
			this.serviceProvider = serviceProvider;

			var drivePaths = systemFolderProvider.GetLogicalDrives();
			var driveIcon = new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Drive.ico")));
			foreach (var drivePath in drivePaths) {
				var driveItem = serviceProvider.GetService<TreeFolderItem>();
				driveItem.Path = drivePath;
				driveItem.Icon = driveIcon;
				TreeItems.Add(driveItem);
			}

			var recentPath = systemFolderProvider.GetRecentFolder();
			var recentIcon = new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Favorites.ico")));
			var recentItem = serviceProvider.GetService<TreeFolderItem>();
			recentItem.Path = recentPath;
			recentItem.Icon = recentIcon;
			TreeItems.Add(recentItem);
		}

		#endregion Public Constructors

		#region Public Methods

		public void Navigate(TreeFolderItem folderItem)
		{
			folderNavigationService.Navigate("FolderPage", folderItem.Path);
		}

		#endregion Public Methods
	}
}