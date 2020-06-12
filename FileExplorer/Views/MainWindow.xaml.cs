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
		private readonly FolderNavigationService folderNavigationService;

		public MainWindow()
		{
			InitializeComponent();
		}

		public MainWindow(MainWindowViewModel vm, IFolderNavigationService folderNavigationService) : this()
		{
			this.vm = vm;
			this.folderNavigationService = folderNavigationService as FolderNavigationService;
			// inject navigation service 
			this.folderNavigationService.NavigationService = FolderFrame.NavigationService;
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
