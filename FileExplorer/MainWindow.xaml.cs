using Cyc.FluentDesign;
using FileExplorer.Models;
using FileExplorer.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace FileExplorer.Views {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RevealWindow {
		private readonly MainWindowViewModel vm;

		public MainWindow()
		{
			InitializeComponent();
			var fileProvider = new FileProvider();
			var systemFolderProvider = new SystemFolderProvider();
			vm = new MainWindowViewModel(fileProvider, systemFolderProvider);
			DataContext = vm;
		}

		protected override void BrowseBack(object sender, ExecutedRoutedEventArgs e)
		{
			base.BrowseBack(sender, e);
		}

		protected override void BrowseForward(object sender, ExecutedRoutedEventArgs e)
		{
			base.BrowseForward(sender, e);
		}

		private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is FolderItem folderItem)) {
				return;
			}
			folderItem.LoadSubFolders();
			vm.CurrentFolder = folderItem;
			// avoid recursive event propagation
			if (e != null) {
				e.Handled = true;
			}
		}
		private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is FolderItem folderItem)) {
				return;
			}
			folderItem.LoadSubFolders();
		}
		private void ListViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListViewItem)?.DataContext is FolderItem folderItem)) {
				return;
			}
			// Cannot change CurrentFolder when ListViewItem's event hasn't finished
			Dispatcher.BeginInvoke(new Action(() => {
				folderItem.LoadSubFolders();
				vm.CurrentFolder = folderItem;
			}));
		}
		private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListBoxItem)?.DataContext is FolderItem folderItem)) {
				return;
			}
			folderItem.LoadSubFolders();
			vm.CurrentFolder = folderItem;
		}
	}
}
