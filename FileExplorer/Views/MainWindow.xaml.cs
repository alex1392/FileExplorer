using Cyc.FluentDesign;
using FileExplorer.Models;
using FileExplorer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
		private readonly IServiceProvider sp;

		public MainWindow()
		{
			InitializeComponent();
		}

		public MainWindow(MainWindowViewModel vm, IServiceProvider sp) : this()
		{
			this.vm = vm;
			this.sp = sp;
			DataContext = vm;
		}

		private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is TreeFolderItem folderItem)) {
				return;
			}
			// load the subfolders for the tree view item to expand
			folderItem.LoadSubFolders();
			// Navigate to the selected folder
			var page = sp.GetService<FolderPage>();
			page.Path = folderItem.Path; // property injection
			FolderFrame.Navigate(page);

			// avoid recursive event propagation
			if (e != null) {
				e.Handled = true;
			}
		}
		private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is TreeFolderItem folderItem)) {
				return;
			}
			folderItem.LoadSubFolders();
		}
		//private void ListViewItem_Selected(object sender, RoutedEventArgs e)
		//{
		//	if (!((sender as ListViewItem)?.DataContext is TreeFolderItem folderItem)) {
		//		return;
		//	}
		//	// Cannot change CurrentFolder when ListViewItem's event hasn't finished
		//	Dispatcher.BeginInvoke(new Action(() => {
		//		folderItem.LoadSubFolders();
		//		//vm.CurrentFolder = folderItem;
		//	}));
		//}
		//private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
		//{
		//	if (!((sender as ListBoxItem)?.DataContext is TreeFolderItem folderItem)) {
		//		return;
		//	}
		//	folderItem.LoadSubFolders();
		//	//vm.CurrentFolder = folderItem;
		//}
	}
}
