using FileExplorer.Models;

using GongSolutions.Wpf.DragDrop;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileExplorer.ViewModels
{
	public class FolderPageViewModel : INotifyPropertyChanged
	{
		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly FolderChildrenProvider folderChildrenProvider;
		private readonly INavigationService navigationService;
		private readonly IServiceProvider serviceProvider;
		private string path;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public ObservableCollection<ListItemViewModel> ListItems { get; } = new ObservableCollection<ListItemViewModel>();

		/// <summary>
		/// Property injection
		/// </summary>
		public string Path
		{
			get => path;
			internal set
			{
				// can only be set once
				if (path != null || path == value)
				{
					return;
				}
				path = value;

				SetupListItems(path);

				var info = fileProvider.GetFileSystemInfo(path);
				Title = info.Name;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
			}
		}

		public string Title { get; set; }
		public IDropTarget FileDropHandler { get; }
		public IDragSource FileDragHandler { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// for xaml designer
		/// </summary>
		public FolderPageViewModel()
		{
		}

		public FolderPageViewModel(FolderChildrenProvider folderChildrenProvider, IFileProvider fileProvider, INavigationService navigationService, IServiceProvider serviceProvider, FileDropHandler fileDropHandler, FileDragHandler fileDragHandler)
		{
			this.folderChildrenProvider = folderChildrenProvider;
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
			this.serviceProvider = serviceProvider;
			FileDropHandler = fileDropHandler;
			FileDragHandler = fileDragHandler;
		}

		#endregion Public Constructors

		#region Public Methods

		public void Navigate(ListFolderItem folderItem)
		{
			navigationService.Navigate("FolderPage", folderItem.Path);
		}

		public void Navigate(Item pathItem)
		{
			navigationService.Navigate("FolderPage", pathItem.Path);
		}

		public void Navigate(string path)
		{
			navigationService.Navigate("FolderPage", path);
		}

		#endregion Public Methods

		#region Private Methods

		private void SetupListItems(string path)
		{
			var (folderPaths, filePaths) = fileProvider.GetChildren(path);
			foreach (var folderPath in folderPaths)
			{
				var folderItem = serviceProvider.GetService<ListFolderItemViewModel>();
				folderItem.Path = folderPath;
				ListItems.Add(folderItem);
			}
			foreach (var filePath in filePaths)
			{
				var fileItem = serviceProvider.GetService<ListFileItemViewModel>();
				fileItem.Path = filePath;
				ListItems.Add(fileItem);
			}
		}

		#endregion Private Methods
	}
}