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
				Vm.Path = value; // property injection
			}
		}

		public FolderPageViewModel Vm {
			get {
				// lazy loading dependency
				return vm ?? (vm = serviceProvider.GetService<FolderPageViewModel>());
			}
		}

		public FolderPage()
		{
			InitializeComponent();
			this.Loaded += FolderPage_Loaded;
		}


		public FolderPage(IServiceProvider serviceProvider) : this()
		{
			this.serviceProvider = serviceProvider;
		}

		private void FolderPage_Loaded(object sender, RoutedEventArgs e)
		{
			DataContext = Vm;
		}

		private void ListViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListViewItem)?.DataContext is ListFolderItem folderItem)) {
				return;
			}
			Vm.Navigate(folderItem);
		}

		private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListBoxItem)?.DataContext is string path)) {
				return;
			}
			Vm.Navigate(path);
		}
	}
}
