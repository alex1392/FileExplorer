using FileExplorer.Models;
using FileExplorer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.ComponentModel;
using System.Windows.Controls;
using Navigation = System.Windows.Navigation;

namespace FileExplorer.Views {

	public class NavigationService : INavigationService {

		#region Private Fields

		private readonly IServiceProvider serviceProvider;
		private Navigation::NavigationService navigationService;

		#endregion Private Fields

		public event EventHandler<string> Navigated;

		#region Public Properties


		// set navigation service when the first time you get it
		public Navigation::NavigationService WpfNavigationService {
			get {
				if (navigationService != null) {
					return navigationService;
				}
				navigationService = serviceProvider.GetService<MainWindow>().FolderFrame.NavigationService;
				// propagate navigated event
				navigationService.Navigated += (sender, e) => {
					Navigated?.Invoke(sender, (e.Content as FolderPage)?.Path);
				};
				return navigationService;
			}
		}

		/// <summary>
		/// Get or set the current content
		/// </summary>
		public object Content {
			get => WpfNavigationService.Content;
			set {
				if (WpfNavigationService.Content == value) {
					return;
				}
				WpfNavigationService.Content = value;
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public NavigationService(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		#endregion Public Constructors

		#region Public Methods
		public void GoBack()
		{
			if (WpfNavigationService.CanGoBack) {
				WpfNavigationService.GoBack();
			}
		}

		public void GoForward()
		{
			if (WpfNavigationService.CanGoForward) {
				WpfNavigationService.GoForward();
			}
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
			WpfNavigationService.Navigate(page);
		}

		#endregion Public Methods

	}
}