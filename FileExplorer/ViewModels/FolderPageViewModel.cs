using FileExplorer.DataVirtualization;
using FileExplorer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

using IO = System.IO;

namespace FileExplorer.ViewModels {

	public class FolderPageViewModel : INotifyPropertyChanged {

		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly FolderChildrenProvider folderChildrenProvider;
		private readonly IFolderNavigationService folderNavigationService;
		private readonly IServiceProvider serviceProvider;
		private string path;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public ObservableCollection<ListItem> ListItems { get; } = new ObservableCollection<ListItem>();

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

				SetupListItems(path);

				// TODO: is title corrent??
				var info = fileProvider.GetFileSystemInfo(path);
				Title = info.Name;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
			}
		}

		private void SetupListItems(string path)
		{
			var (folderPaths, filePaths) = fileProvider.GetChildren(path);
			foreach (var folderPath in folderPaths) {
				var folderItem = serviceProvider.GetService<ListFolderItem>();
				folderItem.Path = folderPath;
				ListItems.Add(folderItem);
			}
			foreach (var filePath in filePaths) {
				var folderItem = serviceProvider.GetService<ListFileItem>();
				folderItem.Path = filePath;
				ListItems.Add(folderItem);
			}
		}
		public string Title { get; set; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// for xaml designer
		/// </summary>
		public FolderPageViewModel()
		{
		}

		public FolderPageViewModel(FolderChildrenProvider folderChildrenProvider, IFileProvider fileProvider, IFolderNavigationService folderNavigationService, IServiceProvider serviceProvider)
		{
			this.folderChildrenProvider = folderChildrenProvider;
			this.fileProvider = fileProvider;
			this.folderNavigationService = folderNavigationService;
			this.serviceProvider = serviceProvider;
		}

		#endregion Public Constructors

		#region Public Methods

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

		#endregion Public Methods

		#region Private Methods

		

		#endregion Private Methods
	}
}