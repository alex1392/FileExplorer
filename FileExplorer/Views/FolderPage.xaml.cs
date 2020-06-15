using FileExplorer.Models;
using FileExplorer.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FileExplorer.Views {

	/// <summary>
	/// Interaction logic for FolderPage.xaml
	/// </summary>
	public partial class FolderPage : Page {

		#region Private Fields

		private readonly FolderPageViewModel vm;
		private string path;

		#endregion Private Fields

		#region Public Properties

		public string Path {
			get => path;
			set {
				// can only be set once
				if (path != null || path == value) {
					return;
				}
				path = value;
				vm.Path = value; // property injection
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

		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (!((sender as ListViewItem)?.DataContext is ListFolderItem folderItem)) {
				return;
			}
			vm.Navigate(folderItem);
		}

		public void ToggleGroupByType()
		{
			if (!(CollectionViewSource.GetDefaultView(ItemsListView.ItemsSource) is CollectionView collectionView)) {
				return;
			}
			var groupDescription = new PropertyGroupDescription(nameof(ListItem.TypeDescription));
			collectionView.GroupDescriptions.Clear();
			collectionView.GroupDescriptions.Add(groupDescription);
		}

		public void UnToggleGroupByType()
		{
			if (!(CollectionViewSource.GetDefaultView(ItemsListView.ItemsSource) is CollectionView collectionView)) {
				return;
			}
			collectionView.GroupDescriptions.Clear();
		}

		#endregion Private Methods
	}
}