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

		public string Path { get; private set; }
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

		public MainWindowViewModel(ISystemFolderProvider systemFolderProvider, INavigationService navigationService, IServiceProvider serviceProvider)
		{
			this.systemFolderProvider = systemFolderProvider;
			this.navigationService = navigationService;
			this.serviceProvider = serviceProvider;
			navigationService.Navigated += NavigationService_Navigated;
			navigationService.NavigatedPageLoaded += NavigationService_NavigatedPageLoaded;

			SetupHomePage();
			SetupTreeItems();

			GoBackCommand = new GoBackCommand(navigationService);
			GoForwardCommand = new GoForwardCommand(navigationService);
			RefreshCommand = new RefreshCommand(navigationService);
			GoUpCommand = new GoUpCommand(navigationService);
			GoHomeCommand = new GoHomeCommand(navigationService, HomePage);
		}

		private void SetupHomePage()
		{
			HomePage = serviceProvider.GetService<TreePageItem>();
			HomePage.Uri = new Uri("/Views/HomePage.xaml", UriKind.Relative);
			HomePage.IconKey = "Home";
			TreeItems.Add(HomePage);
		}

		#endregion Public Constructors

		#region Public Methods

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

		private IEnumerable<Item> GetPathItems(string path)
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

		private void NavigationService_Navigated(object sender, string path)
		{
			Path = path;
			PathItems = GetPathItems(path);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathItems)));
		}

		private void NavigationService_NavigatedPageLoaded(object sender, EventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavigationHistroy)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentContent)));
		}

		private void SetupTreeItems()
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

		#endregion Private Methods
	}
}