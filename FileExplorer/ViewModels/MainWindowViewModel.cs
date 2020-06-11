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
	public class MainWindowViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<TreeFolderItem> TreeItems { get; set; } = new ObservableCollection<TreeFolderItem>();

		//private TreeFolderItem currentFolder;
		//public TreeFolderItem CurrentFolder {
		//	get => currentFolder;
		//	set {
		//		if (currentFolder != value) {
		//			currentFolder = value;
		//			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFolder)));
		//			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentItems)));
		//			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathItems)));
		//		}
		//	}
		//}

		//public VirtualizingCollection<Item> CurrentItems => CurrentFolder?.Children;

		//public List<TreeFolderItem> PathItems {
		//	get {
		//		var list = new List<TreeFolderItem>();
		//		var folder = currentFolder;
		//		while (folder != null) {
		//			list.Add(folder);
		//			folder = folder.Parent;
		//		}
		//		list.Reverse();
		//		return list;
		//	}
		//}

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
				TreeItems.Add(new TreeFolderItem(drivePath, fileProvider, driveIcon));
			}

			var recentPath = systemFolderProvider.GetRecentFolder();
			var recentIcon = new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Favorites.ico")));
			TreeItems.Add(new TreeFolderItem(recentPath, fileProvider, recentIcon));
		}
	}
}
