﻿using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Navigation;

using Navigation = System.Windows.Navigation;

namespace FileExplorer.Views.Services {

	public class NavigationService : INavigationService {

		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly IServiceProvider serviceProvider;
		private Frame internalFrame;
		private Navigation::NavigationService internalNavigationService;

		#endregion Private Fields

		#region Public Events

		public event EventHandler<string> Navigated;

		public event EventHandler NavigatedPageLoaded;

		#endregion Public Events

		#region Public Properties

		public IEnumerable BackStack => InternalFrame.BackStack;
		public bool CanGoBack => InternalNavigationService.CanGoBack;

		public bool CanGoForward => InternalNavigationService.CanGoForward;

		public bool CanGoUp {
			get {
				if (GetParentPath() == null) {
					return false;
				}
				return true;
			}
		}

		/// <summary>
		/// Get or set the current content
		/// </summary>
		public object Content {
			get => InternalNavigationService.Content;
			set {
				if (InternalNavigationService.Content == value) {
					return;
				}
				InternalNavigationService.Content = value;
			}
		}

		public IEnumerable ForwardStack => InternalFrame.ForwardStack;

		public Frame InternalFrame {
			get {
				//Lazy initialization of dependency
				if (internalFrame == null) {
					internalFrame = serviceProvider.GetService<MainWindow>().FolderFrame;
				}
				return internalFrame;
			}
		}

		public Navigation::NavigationService InternalNavigationService {
			get {
				// Lazy initialization of dependency
				if (internalNavigationService == null) {
					internalNavigationService = serviceProvider.GetService<MainWindow>().FolderFrame.NavigationService;
					// propagate navigated event
					internalNavigationService.Navigated += InternalNavigationService_Navigated;
				}
				return internalNavigationService;
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public NavigationService(IServiceProvider serviceProvider, IFileProvider fileProvider)
		{
			this.serviceProvider = serviceProvider;
			this.fileProvider = fileProvider;
		}

		#endregion Public Constructors

		#region Public Methods

		public void GoBack()
		{
			if (!InternalNavigationService.CanGoBack) {
				return;
			}
			InternalNavigationService.GoBack();
		}

		public void GoForward()
		{
			if (!InternalNavigationService.CanGoForward) {
				return;
			}
			InternalNavigationService.GoForward();
		}

		public void GoUp()
		{
			if (!CanGoUp) {
				return;
			}
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
				"FolderPage" => serviceProvider.GetService<FolderPage>(),
				_ => throw new InvalidOperationException("Cannot recognize given pageKey.")
			};
			// check if destination path is exist
			var fileProvider = serviceProvider.GetService<IFileProvider>();
			if (!fileProvider.IsDirectoryExists(path)) {
				serviceProvider.GetService<IDialogService>().ShowMessage("Specified folder path does not exist.");
				return;
			}
			page.Path = path;
			InternalNavigationService.Navigate(page);
		}

		public void Navigate(object destination)
		{
			InternalNavigationService.Navigate(destination);
		}

		public void Navigate(Uri uri)
		{
			InternalNavigationService.Navigate(uri);
		}

		public void Refresh()
		{
			InternalNavigationService.Refresh();
		}

		#endregion Public Methods

		#region Private Methods

		private string GetParentPath()
		{
			if (!(Content is FolderPage folderPage)) {
				return null;
			}
			var path = folderPage.Path;
			return fileProvider.GetParent(path);
		}

		private void InternalNavigationService_Navigated(object sender, NavigationEventArgs e)
		{
			if (!(e.Content is Page page)) {
				return;
			}
			Navigated?.Invoke(sender, (page as FolderPage)?.Path);
			page.Loaded += (sender, e) => {
				NavigatedPageLoaded?.Invoke(sender, null);
			};
		}

		#endregion Private Methods
	}
}