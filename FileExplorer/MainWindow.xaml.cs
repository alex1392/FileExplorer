using FileExplorer.DataVirtualization;
using FileExplorer.Models;
using FileExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FileExplorer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private readonly MainWindowViewModel vm;

		public MainWindow()
		{
			InitializeComponent();
			var fileProvider = new FileProvider();
			var systemFolderProvider = new SystemFolderProvider();
			vm = new MainWindowViewModel(fileProvider, systemFolderProvider);
			DataContext = vm;
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
