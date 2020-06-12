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
		private MainWindowViewModel vm;
		private readonly IServiceProvider serviceProvider;

		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
		}
		public MainWindow(IServiceProvider serviceProvider) : this()
		{
			this.serviceProvider = serviceProvider;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// always load dependencies of an object outside of its constructor if it is possible, this way circular dependency can be avoided
			vm = serviceProvider.GetService<MainWindowViewModel>();
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
			vm.Navigate(folderItem);
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
		
	}
}
