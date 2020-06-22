using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using IO = System.IO;

namespace FileExplorer.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Private Fields

		private readonly INavigationService navigationService;
		private readonly IServiceProvider serviceProvider;
		private readonly IFileProvider fileProvider;
		private readonly UndoRedoManager undoRedoManager;
		private readonly ISystemFolderProvider systemFolderProvider;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public object CurrentContent => navigationService.Content;

		public ICommand GoBackCommand { get; private set; }

		public ICommand GoForwardCommand { get; private set; }

		public ICommand GoUpCommand { get; private set; }
		public ICommand GoHomeCommand { get; private set; }
		public ICommand RefreshCommand { get; private set; }

		public IEnumerable<object> NavigationHistroy
		{
			get
			{
				var list = new List<object>();
				if (navigationService.BackStack != null)
				{
					foreach (var item in navigationService.BackStack)
					{
						list.Add(item);
					}
				}
				list.Reverse();
				list.Add(navigationService.Content);
				if (navigationService.ForwardStack != null)
				{
					foreach (var item in navigationService.ForwardStack)
					{
						list.Add(item);
					}
				}

				return list;
			}
		}

		public string CurrentPath { get; private set; }
		public IEnumerable<Item> PathItems { get; private set; }
		public ObservableCollection<ITreeItem> TreeItems { get; private set; } = new ObservableCollection<ITreeItem>();
		public TreePageItem HomePage { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// for xaml designer
		/// </summary>
		public MainWindowViewModel()
		{
		}

		public MainWindowViewModel(ISystemFolderProvider systemFolderProvider, INavigationService navigationService, IServiceProvider serviceProvider, IFileProvider fileProvider, UndoRedoManager undoRedoManager, GoBackCommand goBackCommand, GoForwardCommand goForwardCommand, GoUpCommand goUpCommand, RefreshCommand refreshCommand, GoHomeCommand goHomeCommand)
		{
			this.systemFolderProvider = systemFolderProvider;
			this.navigationService = navigationService;
			this.serviceProvider = serviceProvider;
			this.fileProvider = fileProvider;
			this.undoRedoManager = undoRedoManager;
			navigationService.Navigated += NavigationService_Navigated;
			navigationService.NavigatedPageLoaded += NavigationService_NavigatedPageLoaded;

			SetupHomePage();
			SetupTreeItems();
			SetupCommands();

			void SetupHomePage()
			{
				HomePage = serviceProvider.GetService<TreePageItem>();
				HomePage.Uri = new Uri("/Views/HomePage.xaml", UriKind.Relative);
				HomePage.IconKey = "Home";
				TreeItems.Add(HomePage);
			}
			void SetupTreeItems()
			{
				var funcs = new List<Func<string>>
				{
					systemFolderProvider.GetDesktop,
					systemFolderProvider.GetRecent,
					systemFolderProvider.GetDownloads,
					systemFolderProvider.GetDocuments,
					systemFolderProvider.GetPictures,
					systemFolderProvider.GetMusic,
					systemFolderProvider.GetVideos,
				};
				foreach (var func in funcs)
				{
					var path = func.Invoke();
					SetupTreeItem(path, func.Method.Name.Replace("Get", ""));
				}

				var drivePaths = systemFolderProvider.GetLogicalDrives();
				foreach (var drivePath in drivePaths)
				{
					SetupTreeItem(drivePath, "Drive");
				}

				void SetupTreeItem(string path, string iconKey)
				{
					var item = serviceProvider.GetService<TreeFolderItem>();
					item.Path = path;
					item.IconKey = iconKey;
					TreeItems.Add(item);
				}
			}
			void SetupCommands()
			{
				GoBackCommand = goBackCommand;
				GoForwardCommand = goForwardCommand;
				RefreshCommand = refreshCommand;
				GoUpCommand = goUpCommand;
				goHomeCommand.HomePage = HomePage;
				GoHomeCommand = goHomeCommand;
			}
		}

		#endregion Public Constructors

		#region Public Methods

		public bool Paste(List<string> sourcePaths, string destPath, PasteType type)
		{
			PasteCommand command = type switch
			{
				PasteType.Cut => serviceProvider.GetService<CutPasteCommand>(),
				PasteType.Copy => serviceProvider.GetService<CopyPasteCommand>(),
				_ => throw new NotImplementedException(),
			};
			command.SourcePaths = sourcePaths;
			command.DestPath = destPath;
			undoRedoManager.Execute(command);
			return command.IsExecutionSuccessful;
		}

		public void New(string path)
		{
			var command = serviceProvider.GetService<CreateCommand>();
			command.Path = path;
			undoRedoManager.Execute(command);
		}

		public void Delete(List<string> paths)
		{
			var command = serviceProvider.GetService<DeleteCommand>();
			command.Paths = paths;
			undoRedoManager.Execute(command);
		}

		public bool CanRedo(object parameter) => undoRedoManager.RedoCommand.CanExecute(parameter);

		public bool CanUndo(object parameter) => undoRedoManager.UndoCommand.CanExecute(parameter);

		public void Redo(object parameter) => undoRedoManager.RedoCommand.Execute(parameter);

		public void Undo(object parameter) => undoRedoManager.UndoCommand.Execute(parameter);

		public void Navigate(Uri uri)
		{
			navigationService.Navigate(uri);
		}

		public void Navigate(TreePageItem treePageItem)
		{
			navigationService.Navigate(treePageItem.Uri);
		}

		public void Navigate(Item item)
		{
			navigationService.Navigate("FolderPage", item.Path);
		}

		public void Navigate(string path)
		{
			navigationService.Navigate("FolderPage", path);
		}

		#endregion Public Methods

		#region Private Methods

		private void NavigationService_Navigated(object sender, string path)
		{
			CurrentPath = path;
			PathItems = GetPathItems(path);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathItems)));

			IEnumerable<Item> GetPathItems(string path)
			{
				if (path == null)
				{
					return null;
				}
				var parents = path.Split(IO::Path.DirectorySeparatorChar).Where(s => !string.IsNullOrEmpty(s)).ToList();
				var paths = new string[parents.Count];
				for (var i = 0; i < parents.Count; i++)
				{
					paths[i] = string.Join(IO::Path.DirectorySeparatorChar.ToString(), parents.Take(i + 1));
				}
				return paths.Select(path =>
				{
					var item = serviceProvider.GetService<Item>();
					item.Path = path;
					return item;
				});
			}
		}

		private void NavigationService_NavigatedPageLoaded(object sender, EventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavigationHistroy)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentContent)));
		}

		#endregion Private Methods
	}
}