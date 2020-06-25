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
		private readonly INavigationService navigationService;
		private readonly IServiceProvider serviceProvider;
		private readonly IDialogService dialogService;
		private readonly UndoRedoManager undoRedoManager;
		private readonly IFileWatcherService fileWatcher;
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

				SetupListItems();

				fileWatcher.Path = path;
				fileWatcher.Start();

				var info = fileProvider.GetFileSystemInfo(path);
				Title = info.Name;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
			}
		}

		public string Title { get; set; }
		public IDropTarget FileDropHandler { get; }
		public IDragSource FileDragHandler { get; }

		public RenameDialogCommand RenameDialogCommand { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// for xaml designer
		/// </summary>
		public FolderPageViewModel()
		{
		}

		public FolderPageViewModel(IFileProvider fileProvider, INavigationService navigationService, IServiceProvider serviceProvider, IDialogService dialogService, IFileWatcherService fileWatcher, UndoRedoManager undoRedoManager, FileDropHandler fileDropHandler, FileDragHandler fileDragHandler, RenameDialogCommand renameDialogCommand)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
			this.serviceProvider = serviceProvider;
			this.dialogService = dialogService;
			this.fileWatcher = fileWatcher;
			this.undoRedoManager = undoRedoManager;

			fileDropHandler.CurrentFolderPath = Path;
			FileDropHandler = fileDropHandler;
			FileDragHandler = fileDragHandler;

			RenameDialogCommand = renameDialogCommand;
		}

		#endregion Public Constructors

		#region Public Methods

		public void SetupListItems()
		{
			ListItems.Clear();
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

		#endregion Public Methods
	}
}