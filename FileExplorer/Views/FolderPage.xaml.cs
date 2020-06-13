﻿using FileExplorer.Models;
using FileExplorer.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileExplorer.Views {

	/// <summary>
	/// Interaction logic for FolderPage.xaml
	/// </summary>
	public partial class FolderPage : Page {

		#region Private Fields

		private readonly IServiceProvider serviceProvider;
		private string path;
		private FolderPageViewModel vm;

		#endregion Private Fields

		#region Public Properties

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

		#endregion Public Properties

		#region Public Constructors

		public FolderPage()
		{
			InitializeComponent();
			Loaded += FolderPage_Loaded;
		}

		public FolderPage(IServiceProvider serviceProvider) : this()
		{
			this.serviceProvider = serviceProvider;
		}

		#endregion Public Constructors

		#region Private Methods

		private void FolderPage_Loaded(object sender, RoutedEventArgs e)
		{
			DataContext = Vm;
			//Dispatcher.Invoke(() => {
			//	collectionView = CollectionViewSource.GetDefaultView(ItemsDataGrid.ItemsSource) as CollectionView;
			//	groupDescription = new PropertyGroupDescription(nameof(ListItem.TypeDescription));
			//	collectionView.GroupDescriptions.Add(groupDescription);
			//}, DispatcherPriority.Loaded); // priority need to be less than Render
		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (!((sender as ListViewItem)?.DataContext is ListFolderItem folderItem)) {
				return;
			}
			Vm.Navigate(folderItem);
		}

		#endregion Private Methods
	}
}