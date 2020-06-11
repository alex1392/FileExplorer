using FileExplorer.DataVirtualization;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FileExplorer.ViewModels {
	class MainWindowViewModel : INotifyPropertyChanged {
		private FolderItem currentFolder;

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<FolderItem> TreeItems { get; set; } = new ObservableCollection<FolderItem>();

		public FolderItem CurrentFolder {
			get => currentFolder;
			set {
				if (currentFolder != value) {
					currentFolder = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFolder)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentItems)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathItems)));
				}
			}
		}

		public VirtualizingCollection<Item> CurrentItems => CurrentFolder?.Children;

		public List<FolderItem> PathItems {
			get {
				var list = new List<FolderItem>();
				var folder = currentFolder;
				while (folder != null) {
					list.Add(folder);
					folder = folder.Parent;
				}
				list.Reverse();
				return list;
			}
		}

		/// <summary>
		/// for xaml designer
		/// </summary>
		public MainWindowViewModel()
		{

		}
		public MainWindowViewModel(IFileProvider fileProvider, ISystemFolderProvider systemFolderProvider)
		{
			var drivePaths = systemFolderProvider.GetLogicalDrives();
			var driveIcon = new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Drive.ico")));
			foreach (var drivePath in drivePaths) {
				TreeItems.Add(new FolderItem(drivePath, fileProvider, driveIcon));
			}

			var recentPath = systemFolderProvider.GetRecentFolder();
			var recentIcon = new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Favorites.ico")));
			TreeItems.Add(new FolderItem(recentPath, fileProvider, recentIcon));
		}
	}
}
