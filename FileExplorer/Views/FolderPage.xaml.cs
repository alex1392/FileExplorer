using FileExplorer.Models;
using FileExplorer.ViewModels;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using IO = System.IO;
using ListItem = FileExplorer.Models.ListItem;

namespace FileExplorer.Views
{
	/// <summary>
	/// Interaction logic for FolderPage.xaml
	/// </summary>
	public partial class FolderPage : Page, INotifyPropertyChanged
	{
		#region Private Classes

		private class ItemTypeConverter : IValueConverter
		{
			#region Public Methods

			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				if (!(value is ListItem listItem))
				{
					throw new InvalidOperationException();
				}
				return listItem.TypeDescription;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}

			#endregion Public Methods
		}

		#endregion Private Classes

		#region Private Fields

		private readonly FolderPageViewModel vm;
		private readonly INavigationService navigationService;
		private readonly IFileProvider fileProvider;
		private ICollectionView collectionView;
		private string filterText;
		private bool isGrouping;
		private string path;
		private string currentView = "ListView";

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Private Properties

		private ICollectionView CollectionView => collectionView
			?? (collectionView = CollectionViewSource.GetDefaultView(ItemsListView.ItemsSource));

		#endregion Private Properties

		#region Public Properties

		private string selectedItemPath;

		public string FilterText
		{
			get => filterText;
			set
			{
				if (value == filterText)
				{
					return;
				}
				filterText = value;

				RefreshPage();

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterText)));

				void RefreshPage()
				{
					CollectionView?.Refresh();
				}
			}
		}

		public bool IsGrouping
		{
			get => isGrouping;
			set
			{
				if (value == isGrouping)
				{
					return;
				}
				isGrouping = value;
				if (isGrouping)
				{
					var groupDescription = new PropertyGroupDescription(nameof(ListItemViewModel.Item), new ItemTypeConverter());
					CollectionView?.GroupDescriptions.Clear();
					CollectionView?.GroupDescriptions.Add(groupDescription);
				}
				else
				{
					CollectionView?.GroupDescriptions.Clear();
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGrouping)));
			}
		}

		public string Path
		{
			get => path;
			set
			{
				// can only be set once
				if (path != null || path == value)
				{
					return;
				}
				path = value;
				vm.Path = value; // property injection
			}
		}

		public string CurrentView
		{
			get => currentView;
			set
			{
				if (currentView == value)
				{
					return;
				}
				currentView = value;

				ItemsListView.View = ItemsListView.FindResource(currentView) as ViewBase;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
			}
		}

		public string SelectedItemPath
		{
			get { return selectedItemPath; }
			set
			{
				if (selectedItemPath == value)
				{
					return;
				}
				selectedItemPath = value;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItemPath)));
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public FolderPage()
		{
			InitializeComponent();
			Loaded += FolderPage_Loaded;
		}

		public FolderPage(FolderPageViewModel vm, INavigationService navigationService, IFileProvider fileProvider) : this()
		{
			this.vm = vm;
			this.navigationService = navigationService;
			this.fileProvider = fileProvider;
			DataContext = this.vm;
		}

		#endregion Public Constructors

		#region Public Methods

		public void ChangeView(string key)
		{
			CurrentView = key;
		}

		#endregion Public Methods

		#region Private Methods

		private void FolderPage_Loaded(object sender, RoutedEventArgs e)
		{
			ApplyFilter();

			void ApplyFilter()
			{
				if (CollectionView != null)
				{
					CollectionView.Filter = item =>
					{
						if (string.IsNullOrWhiteSpace(FilterText))
						{
							return true;
						}
						return (item as ListItemViewModel).Item.Name.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
					};
				}
			}
		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is ListViewItem listViewItem))
			{
				return;
			}
			if (e.ChangedButton == MouseButton.Left)
			{
				if (listViewItem.DataContext is ListFolderItemViewModel folderVM)
				{
					navigationService.Navigate(nameof(FolderPage), folderVM.Path);
				}
				else if (listViewItem.DataContext is ListFileItemViewModel fileVM)
				{
					var path = fileVM.Path;
					var ext = IO::Path.GetExtension(path);
					// determine if the file is a shortcut
					if (ext.ToLower() != ".lnk")
					{
						// if not, open file with default application
						fileProvider.OpenFile(fileVM.Path);
						return;
					}

					// if so, retrieve the target path from the shortcut
					var targetPath = fileProvider.GetShortcutTargetPath(path);

					// check if the target path is a folder or a file
					if (fileProvider.IsDirectoryExists(targetPath))
					{
						// if so, redirect to the folder
						navigationService.Navigate(nameof(FolderPage), targetPath);
						return;
					}

					// if not, open the shortcut by shell
					fileProvider.OpenFile(path);
				}
			}
		}

		private void ListViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!(sender is ListViewItem listViewItem) ||
				!(listViewItem.DataContext is ListItemViewModel listItemViewModel))
			{
				return;
			}
			SelectedItemPath = listItemViewModel.Path;
		}

		#endregion Private Methods
	}
}