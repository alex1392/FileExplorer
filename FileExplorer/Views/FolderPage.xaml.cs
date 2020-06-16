using FileExplorer.Models;
using FileExplorer.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		#region Private Fields

		private readonly FolderPageViewModel vm;
		private CollectionView collectionView;
		private string path;
		private List<ListView> listViews;
		private ViewType viewType = ViewType.ListView;

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Private Fields

		#region Public Properties

		public CollectionView CollectionView
		{
			get
			{
				if (collectionView == null)
				{
					collectionView = CollectionViewSource.GetDefaultView(ItemsListView.ItemsSource) as CollectionView;
				}
				return collectionView;
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

		public List<ListView> ListViews
		{
			get
			{
				if (listViews == null)
				{
					listViews = new List<ListView>
					{
						ItemsListView,
						ItemsGridView,
						ItemsTileView,
					};
				}
				return listViews;
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

		#region Public Methods

		public void ToggleGroupByType()
		{
			var groupDescription = new PropertyGroupDescription(nameof(ListItem.TypeDescription));
			CollectionView.GroupDescriptions.Clear();
			CollectionView.GroupDescriptions.Add(groupDescription);
		}

		public void UnToggleGroupByType()
		{
			CollectionView?.GroupDescriptions.Clear();
		}

		#endregion Public Methods

		#region Internal Methods

		internal void ApplyFilter(Predicate<object> listItemFilter)
		{
			CollectionView.Filter = listItemFilter;
		}

		internal void RefreshPage()
		{
			CollectionView.Refresh();
		}

		#endregion Internal Methods

		#region Private Methods

		private void FolderPage_Loaded(object sender, RoutedEventArgs e)
		{

		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (!((sender as ListViewItem)?.DataContext is ListFolderItem folderItem))
			{
				return;
			}
			vm.Navigate(folderItem);
		}

		public ViewType ViewType
		{
			get => viewType;
			set
			{
				if (value == viewType)
				{
					return;
				}
				viewType = value;
				HideListViews();
				switch (viewType)
				{
					case ViewType.ListView:
						ItemsListView.Visibility = Visibility.Visible;
						break;
					case ViewType.GridView:
						ItemsGridView.Visibility = Visibility.Visible;
						break;
					case ViewType.TileView:
						ItemsTileView.Visibility = Visibility.Visible;
						break;
					default:
						break;
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewType)));
			}
		}
		internal void ToggleListView()
		{
			ViewType = ViewType.ListView;
		}


		internal void ToggleGridView()
		{
			ViewType = ViewType.GridView;
		}

		internal void ToggleTileView()
		{
			ViewType = ViewType.TileView;
		}
		private void HideListViews()
		{
			foreach (var view in ListViews)
			{
				view.Visibility = Visibility.Collapsed;
			}
		}

		#endregion Private Methods
	}

	public enum ViewType
	{
		ListView,
		GridView,
		TileView,
	}
}