using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using IO = System.IO;

namespace FileExplorer.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Private Fields

		private readonly IServiceProvider serviceProvider;
		private IEnumerable<Item> pathItems;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public RelayCommand GoBackCommand { get; private set; }
		public RelayCommand GoForwardCommand { get; private set; }
		public RelayCommand GoUpCommand { get; private set; }
		public RelayCommand GoHomeCommand { get; private set; }
		public RelayCommand RefreshCommand { get; private set; }

		public string CurrentPath { get; private set; }

		public IEnumerable<Item> PathItems
		{
			get => pathItems;
			private set
			{
				if (value == pathItems)
				{
					return;
				}
				pathItems = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathItems)));
			}
		}

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

		public MainWindowViewModel(ISystemFolderProvider systemFolderProvider, INavigationService navigationService, IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
			navigationService.Navigated += NavigationService_Navigated;

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
				GoBackCommand = new RelayCommand(
					() => navigationService.GoBack(),
					() => navigationService.CanGoBack);
				GoForwardCommand = new RelayCommand(
					() => navigationService.GoForward(),
					() => navigationService.CanGoForward);
				GoUpCommand = new RelayCommand(
					() => navigationService.GoUp(),
					() => navigationService.CanGoUp);
				navigationService.Navigated += (_, _) =>
				{
					GoBackCommand.RaiseCanExecuteChanged();
					GoForwardCommand.RaiseCanExecuteChanged();
					GoUpCommand.RaiseCanExecuteChanged();
				};

				RefreshCommand = new RelayCommand(
					() => navigationService.Refresh());
				GoHomeCommand = new RelayCommand(
					() => navigationService.Navigate(HomePage.Uri),
					() => HomePage != null);
			}
		}

		#endregion Public Constructors

		#region Private Methods

		private void NavigationService_Navigated(object sender, string path)
		{
			CurrentPath = path;
			PathItems = GetPathItems(path);

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

		#endregion Private Methods
	}
}