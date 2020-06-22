using FileExplorer.Models;
using FileExplorer.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
		private ICollectionView collectionView;
		private string filterText;
		private bool isGrouping;
		private string path;
		private ListView currentView;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Private Properties

		private ICollectionView CollectionView => collectionView
			?? (collectionView = CollectionViewSource.GetDefaultView(ItemsListView.ItemsSource));

		#endregion Private Properties

		#region Public Properties

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

		public ListView CurrentView
		{
			get => currentView ?? (CurrentView = ItemsListView);
			set
			{
				if (currentView == value)
				{
					return;
				}
				currentView = value;

				HideListViews();
				currentView.Visibility = Visibility.Visible;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));

				void HideListViews()
				{
					foreach (var view in new List<ListView>
					{
						ItemsListView,
						ItemsGridView,
						ItemsTileView,
					})
					{
						view.Visibility = Visibility.Collapsed;
					}
				}
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public FolderPage()
		{
			InitializeComponent();
			Loaded += FolderPage_Loaded;
		}

		public FolderPage(FolderPageViewModel vm) : this()
		{
			this.vm = vm;
			DataContext = this.vm;
		}

		#endregion Public Constructors

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
					vm.Navigate(folderVM.Item);
				}
				else if (listViewItem.DataContext is ListFileItemViewModel fileVM)
				{
					// open file with default application
					Process.Start(fileVM.Path);
				}
			}
		}

		#endregion Private Methods
	}
}