using FileExplorer.Models;
using FileExplorer.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileExplorer.Views {

	/// <summary>
	/// Interaction logic for FolderPage.xaml
	/// </summary>
	public partial class FolderPage : Page {

		#region Private Fields

		private string path;
		private readonly FolderPageViewModel vm;

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
			//Dispatcher.Invoke(() => {
			//	collectionView = CollectionViewSource.GetDefaultView(ItemsDataGrid.ItemsSource) as CollectionView;
			//	groupDescription = new PropertyGroupDescription(nameof(ListItem.TypeDescription));
			//	collectionView.GroupDescriptions.Add(groupDescription);
			//}, DispatcherPriority.Loaded); // priority need to be less than Render
		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (!((sender as ListViewItem)?.DataContext is ListFolderItem folderItem)) {
				return;
			}
			vm.Navigate(folderItem);
		}

		#endregion Private Methods
	}
}