using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using IO = System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FileExplorer.ViewModels {

	public class MainWindowViewModel : INotifyPropertyChanged {

		#region Private Fields

		private readonly INavigationService navigationService;
		private readonly IServiceProvider serviceProvider;
		private readonly ISystemFolderProvider systemFolderProvider;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties
		public IEnumerable<object> NavigationHistroy {
			get {
				var list = new List<object>();
				if (navigationService.BackStack != null) {
					foreach (var item in navigationService.BackStack) {
						list.Add(item);
					}

				}
				list.Reverse();
				list.Add(navigationService.Content);
				if (navigationService.ForwardStack != null) {
					foreach (var item in navigationService.ForwardStack) {
						list.Add(item);
					}

				}

				return list;
			}
		}

		public object CurrentContent => navigationService.Content;

		public ICommand GoBackCommand { get; set; }
		public ICommand GoForwardCommand { get; set; }
		public ICommand GoUpCommand { get; set; }
		public IEnumerable<Item> PathItems { get; private set; }
		public ICommand RefreshCommand { get; set; }
		public ObservableCollection<TreeFolderItem> TreeItems { get; set; } = new ObservableCollection<TreeFolderItem>();
		public string Path { get; private set; }

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

			GoBackCommand = new GoBackCommand(navigationService);
			GoForwardCommand = new GoForwardCommand(navigationService);
			RefreshCommand = new RefreshCommand(navigationService);
			GoUpCommand = new GoUpCommand(navigationService);
			SetupTreeItems();
		}

		public void Navigate(Uri uri)
		{
			navigationService.Navigate(uri);
		}

		public void Navigate(object destination)
		{
			navigationService.Navigate(destination);
		}

		#endregion Public Constructors

		#region Public Methods

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
			if (path == null) {
				return null;
			}
			var parents = path.Split(IO::Path.DirectorySeparatorChar).Where(s => !string.IsNullOrEmpty(s)).ToList();
			var paths = new string[parents.Count];
			for (var i = 0; i < parents.Count; i++) {
				paths[i] = string.Join(IO::Path.DirectorySeparatorChar.ToString(), parents.Take(i + 1));
			}
			return paths.Select(path => {
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
			var drivePaths = systemFolderProvider.GetLogicalDrives();
			foreach (var drivePath in drivePaths) {
				var driveItem = serviceProvider.GetService<TreeFolderItem>();
				driveItem.Path = drivePath;
				driveItem.IconKey = "Drive";
				TreeItems.Add(driveItem);
			}

			var recentPath = systemFolderProvider.GetRecentFolder();
			var recentItem = serviceProvider.GetService<TreeFolderItem>();
			recentItem.Path = recentPath;
			recentItem.IconKey = "Favorites";
			TreeItems.Add(recentItem);
		}

		#endregion Private Methods
	}
}