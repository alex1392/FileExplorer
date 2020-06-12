using FileExplorer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace FileExplorer.ViewModels {
	public class MainWindowViewModel : INotifyPropertyChanged {
		private readonly IFileProvider fileProvider;
		private readonly ISystemFolderProvider systemFolderProvider;
		private readonly IFolderNavigationService folderNavigationService;

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<TreeFolderItem> TreeItems { get; set; } = new ObservableCollection<TreeFolderItem>();


		/// <summary>
		/// for xaml designer
		/// </summary>
		public MainWindowViewModel()
		{

		}
		
		public MainWindowViewModel(IFileProvider fileProvider, ISystemFolderProvider systemFolderProvider, IFolderNavigationService folderNavigationService)
		{
			this.fileProvider = fileProvider;
			this.systemFolderProvider = systemFolderProvider;
			this.folderNavigationService = folderNavigationService;

			var drivePaths = systemFolderProvider.GetLogicalDrives();
			var driveIcon = new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Drive.ico")));
			foreach (var drivePath in drivePaths) {
				TreeItems.Add(new TreeFolderItem(drivePath, fileProvider, driveIcon));
			}

			var recentPath = systemFolderProvider.GetRecentFolder();
			var recentIcon = new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Favorites.ico")));
			TreeItems.Add(new TreeFolderItem(recentPath, fileProvider, recentIcon));
		}

		public void Navigate(TreeFolderItem folderItem)
		{
			folderNavigationService.Navigate("FolderPage", folderItem.Path);
		}
	}
}
