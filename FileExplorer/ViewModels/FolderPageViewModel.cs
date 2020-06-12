using FileExplorer.DataVirtualization;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using IO = System.IO;
using System.Linq;

namespace FileExplorer.ViewModels {
	public class FolderPageViewModel : INotifyPropertyChanged {
		private readonly FolderChildrenProvider folderChildrenProvider;
		private readonly IFileProvider fileProvider;
		private readonly IFolderNavigationService folderNavigationService;
		private string path;

		public event PropertyChangedEventHandler PropertyChanged;

		public VirtualizingCollection<Item> ListItems { get; private set; }

		public IEnumerable<Item> PathItems { get; private set; }

		/// <summary>
		/// Property injection
		/// </summary>
		public string Path {
			get => path;
			internal set {
				// can only be set once
				if (path != null || path == value) {
					return;
				}
				path = value;

				folderChildrenProvider.Path = path;
				ListItems = new VirtualizingCollection<Item>(folderChildrenProvider, 20);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListItems)));

				PathItems = GetPathItems(path);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathItems)));

				var info = fileProvider.GetFileSystemInfo(path);
				Title = info.Name;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
			}
		}

		public string Title { get; set; }

		/// <summary>
		/// for xaml designer
		/// </summary>
		public FolderPageViewModel()
		{

		}

		public FolderPageViewModel(FolderChildrenProvider folderChildrenProvider, IFileProvider fileProvider, IFolderNavigationService folderNavigationService)
		{
			this.folderChildrenProvider = folderChildrenProvider;
			this.fileProvider = fileProvider;
			this.folderNavigationService = folderNavigationService;
		}

		public void Navigate(ListFolderItem folderItem)
		{
			folderNavigationService.Navigate("FolderPage", folderItem.Path);
		}

		public void Navigate(Item pathItem)
		{
			folderNavigationService.Navigate("FolderPage", pathItem.Path);
		}

		public void Navigate(string path)
		{
			folderNavigationService.Navigate("FolderPage", path);
		}
		private IEnumerable<Item> GetPathItems(string path)
		{
			var parents = path.Split(IO::Path.DirectorySeparatorChar).Where(s => !string.IsNullOrEmpty(s)).ToList();
			var paths = new string[parents.Count];
			for (var i = 0; i < parents.Count; i++) {
				paths[i] = string.Join(IO::Path.DirectorySeparatorChar.ToString(), parents.Take(i + 1));
			}
			return paths.Select(path => new Item(path, fileProvider));
		}

	}
}
