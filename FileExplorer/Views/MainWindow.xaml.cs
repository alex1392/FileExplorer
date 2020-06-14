﻿using Cyc.FluentDesign;

using FileExplorer.Models;
using FileExplorer.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FileExplorer.Views {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RevealWindow {
		private readonly MainWindowViewModel vm;

		#region Public Constructors

		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
		}

		public MainWindow(MainWindowViewModel vm) : this()
		{
			this.vm = vm;
			DataContext = this.vm;
		}

		#endregion Public Constructors

		#region Private Methods

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
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
				vm.Navigate(PathTextBox.Text);
			}
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
			vm.Navigate(folderItem);
			// avoid recursive event propagation
			if (e != null) {
				e.Handled = true;
			}
		}

		#endregion Private Methods

		private void HistoryItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!(sender is ComboBoxItem comboBoxItem) || !(comboBoxItem.DataContext is JournalEntry entry)) {
				return;
			}
			var delta = NavigationComboBox.Items.IndexOf(entry) - NavigationComboBox.Items.IndexOf(vm.CurrentContent);
			while (delta > 0) {
				vm.GoForwardCommand.Execute(null);
				delta--;
			}
			while (delta < 0) {
				vm.GoBackCommand.Execute(null);
				delta++;
			}
		}

		private void PathItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListBoxItem)?.DataContext is Item item)) {
				return;
			}
			if (item.Path == vm.Path) {
				return;
			}
			vm.Navigate(item);
		}
	}
}