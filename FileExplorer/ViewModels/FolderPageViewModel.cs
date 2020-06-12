﻿using FileExplorer.DataVirtualization;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FileExplorer.ViewModels {

	public class FolderPageViewModel : INotifyPropertyChanged {
		private readonly FolderChildrenProvider folderChildrenProvider;
		private readonly IFileProvider fileProvider;
		private readonly IFolderNavigationService folderNavigationService;
		private string path;

		public event PropertyChangedEventHandler PropertyChanged;

		public VirtualizingCollection<Item> ListItems { get; private set; }

		public IEnumerable<string> PathItems { get; private set; }

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

				// TODO: set full path for each pathItems
				PathItems = path.Split(System.IO.Path.DirectorySeparatorChar).Where(s => !string.IsNullOrEmpty(s));
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
			folderNavigationService.Navigate(folderItem.Path);
		}

		public void Navigate(string path)
		{
			folderNavigationService.Navigate(path);
		}

	}
}
