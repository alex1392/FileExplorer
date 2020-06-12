using FileExplorer.Models;
using FileExplorer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileExplorer.Views {
	/// <summary>
	/// Interaction logic for FolderPage.xaml
	/// </summary>
	public partial class FolderPage : Page {
		private FolderPageViewModel vm;
		private readonly IServiceProvider serviceProvider;
		private string path;

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

		public FolderPage()
		{
			InitializeComponent();
			this.Loaded += FolderPage_Loaded;
		}

		private void FolderPage_Loaded(object sender, RoutedEventArgs e)
		{
			this.vm = serviceProvider.GetService<FolderPageViewModel>();
			DataContext = vm;
		}

		public FolderPage(IServiceProvider serviceProvider) : this()
		{
			this.serviceProvider = serviceProvider;
		}

		private void ListViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListViewItem)?.DataContext is ListFolderItem folderItem)) {
				return;
			}
			vm.Navigate(folderItem);
		}

		private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListBoxItem)?.DataContext is string path)) {
				return;
			}
			vm.Navigate(path);
		}
	}
}
