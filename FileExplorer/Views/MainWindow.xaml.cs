﻿using Cyc.FluentDesign;

using FileExplorer.Models;
using FileExplorer.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FileExplorer.Views {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RevealWindow {

		#region Private Fields

		private readonly IServiceProvider serviceProvider;
		private MainWindowViewModel vm;

		#endregion Private Fields

		#region Public Properties

		public MainWindowViewModel Vm {
			get {
				return vm ?? (vm = serviceProvider.GetService<MainWindowViewModel>());
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
		}

		public MainWindow(IServiceProvider serviceProvider) : this()
		{
			// always load dependencies of an object outside of its constructor if it is possible, this way circular dependency can be avoided
			this.serviceProvider = serviceProvider;
		}

		#endregion Public Constructors

		#region Private Methods

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			DataContext = Vm;
		}

		private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is TreeFolderItem folderItem)) {
				return;
			}
			folderItem.LoadSubFolders();
		}

		private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is TreeFolderItem folderItem)) {
				return;
			}
			// load the subfolders for the tree view item to expand
			folderItem.LoadSubFolders();
			// Navigate to the selected folder
			Vm.Navigate(folderItem);
			// avoid recursive event propagation
			if (e != null) {
				e.Handled = true;
			}
		}

		#endregion Private Methods

		private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListBoxItem)?.DataContext is Item item)) {
				return;
			}
			Vm.Navigate(item);
		}

		private void PathListBox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) {
				PathTextBox.Visibility = Visibility.Visible;
			}
			// TODO: remove focus when mouse click on any other area
		}

		private void PathTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			// TODO: prevent this event when pressing enter on the message box
			if (e.Key == Key.Enter) {
				Vm.Navigate(PathTextBox.Text);
			}
		}
	}
}