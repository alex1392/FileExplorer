using Cyc.FluentDesign;

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

		#region Private Fields

		private readonly IServiceProvider serviceProvider;
		private readonly MainWindowViewModel vm;

		#endregion Private Fields

		#region Public Constructors

		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
		}

		public MainWindow(MainWindowViewModel vm, IServiceProvider serviceProvider) : this()
		{
			this.vm = vm;
			this.serviceProvider = serviceProvider;
			DataContext = this.vm;
		}

		#endregion Public Constructors

		#region Private Methods

		private void GroupToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			if (!(FolderFrame.Content is FolderPage folderPage)) {
				return;
			}
			folderPage.ToggleGroupByType();
		}

		private void GroupToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!(FolderFrame.Content is FolderPage folderPage)) {
				return;
			}
			folderPage.UnToggleGroupByType();
		}

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

		private bool ListItemFilter(object item)
		{
			if (string.IsNullOrWhiteSpace(searchTextBox.Text)) {
				return true;
			}
			return (item as ListItem).Name.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			vm.Navigate(new Uri("/Views/HomePage.xaml", UriKind.Relative));
			var navigationService = serviceProvider.GetService<INavigationService>();
			navigationService.Navigated += NavigationService_Navigated;
			navigationService.NavigatedPageLoaded += NavigationService_NavigatedPageLoaded;
		}

		private void NavigationService_Navigated(object sender, string e)
		{
			// reset filter and group
			GroupToggleButton.IsChecked = false;
			searchTextBox.Text = null;
		}

		private void NavigationService_NavigatedPageLoaded(object sender, EventArgs e)
		{
			// apply new filter
			if (!(FolderFrame.Content is FolderPage folderPage)) {
				return;
			}
			folderPage.ApplyFilter(ListItemFilter);
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

		private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!(FolderFrame.Content is FolderPage folderPage)) {
				return;
			}
			folderPage.RefreshPage();
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
	}
}