﻿using FileExplorer.Models;
using FileExplorer.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections;
using System.IO;
using System.Windows.Controls;
using System.Windows.Navigation;

using Navigation = System.Windows.Navigation;

namespace FileExplorer.Views.Services
{
	public class FolderNavigationService : INavigationService
	{
		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly IDialogService dialogService;
		private readonly IServiceProvider serviceProvider;
		private MainWindow mainWindow;

		#endregion Private Fields
		private MainWindow MainWindow => mainWindow
			?? (mainWindow = serviceProvider.GetService<MainWindow>());

		#region Public Events

		public event EventHandler<string> Navigated;

		public event EventHandler NavigatedPageLoaded;

		public event EventHandler GoBackCompleted;

		public event EventHandler GoForwardCompleted;

		#endregion Public Events

		#region Public Properties

		/// <inheritdoc/>
		public object Content
		{
			get => InternalNavigationService?.Content;
			set
			{
				if (InternalNavigationService == null ||
					InternalNavigationService.Content == value)
				{
					return;
				}
				InternalNavigationService.Content = value;
			}
		}


		private Frame InternalFrame => (MainWindow.mainTabControl.SelectedItem as TabContentUserControl)?.FolderFrame;

		private Navigation::NavigationService InternalNavigationService => InternalFrame?.NavigationService;

		public bool CanGoUp => GetParentPath() != null;
		public IEnumerable ForwardStack => InternalFrame?.ForwardStack;
		public IEnumerable BackStack => InternalFrame?.BackStack;
		public bool CanGoBack => InternalNavigationService?.CanGoBack ?? false;
		public bool CanGoForward => InternalNavigationService?.CanGoForward ?? false;

		#endregion Public Properties

		#region Public Constructors

		public FolderNavigationService(IServiceProvider serviceProvider, IFileProvider fileProvider, IDialogService dialogService)
		{
			this.serviceProvider = serviceProvider;
			this.fileProvider = fileProvider;
			this.dialogService = dialogService;
		}

		#endregion Public Constructors

		#region Public Methods

		public void GoBack()
		{
			InternalNavigationService.Navigated += InternalGoBackCompleted;
			InternalNavigationService.GoBack();

			void InternalGoBackCompleted(object sender, NavigationEventArgs e)
			{
				GoBackCompleted?.Invoke(this, EventArgs.Empty);
				InternalNavigationService.Navigated -= InternalGoBackCompleted;
			}
		}

		public void GoForward()
		{
			InternalNavigationService.Navigated += InternalGoForwardCompleted;
			InternalNavigationService.GoForward();

			void InternalGoForwardCompleted(object sender, NavigationEventArgs e)
			{
				GoForwardCompleted?.Invoke(this, EventArgs.Empty);
				InternalNavigationService.Navigated -= InternalGoForwardCompleted;
			}
		}

		public void GoUp()
		{
			var parentPath = GetParentPath();
			var parentFolderPage = serviceProvider.GetService<FolderPage>();
			parentFolderPage.Path = parentPath;
			InternalNavigationService.Navigate(parentFolderPage);
		}

		public void Navigate(string pageKey, string path)
		{
			var page = pageKey switch
			{
				// TODO: make a key registry ??
				nameof(FolderPage) => serviceProvider.GetService<FolderPage>(),
				_ => throw new InvalidOperationException("Cannot recognize given pageKey.")
			};
			// check if destination path is exist
			if (!fileProvider.IsDirectoryExists(path))
			{
				dialogService.ShowMessage("Specified folder path does not exist.");
				return;
			}
			page.Path = path;
			InternalNavigationService.Navigate(page);
		}

		public void Navigate(Uri uri)
		{
			InternalNavigationService.Navigate(uri);
		}

		public void Refresh()
		{
			if (Content is FolderPage folderPage &&
				folderPage.DataContext is FolderPageViewModel vm)
			{
				vm.SetupListItems();
			}
			InternalNavigationService.Refresh();
		}

		#endregion Public Methods

		#region Private Methods

		private string GetParentPath()
		{
			if (!(Content is FolderPage folderPage))
			{
				return null;
			}
			var path = folderPage.Path;
			return Path.GetDirectoryName(path);
		}

		private void InternalNavigationService_Navigated(object sender, NavigationEventArgs e)
		{
			if (!(e.Content is Page page))
			{
				return;
			}
			Navigated?.Invoke(sender, (page as FolderPage)?.Path);
			page.Loaded += (sender, e) =>
			{
				NavigatedPageLoaded?.Invoke(sender, null);
			};
		}

		#endregion Private Methods

		internal void AddFrame(Frame folderFrame)
		{
			// propagate navigated event
			folderFrame.NavigationService.Navigated += InternalNavigationService_Navigated;
		}
	}
}