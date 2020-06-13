using FileExplorer.Models;

using Microsoft.Extensions.DependencyInjection;

using System;

using Navigation = System.Windows.Navigation;

namespace FileExplorer.Views {

	public class NavigationService : INavigationService {

		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly IServiceProvider serviceProvider;
		private Navigation::NavigationService wpfNavigationService;

		#endregion Private Fields

		#region Public Events

		public event EventHandler<string> Navigated;

		#endregion Public Events

		#region Public Properties

		public bool CanGoBack => WpfNavigationService.CanGoBack;

		public bool CanGoForward => WpfNavigationService.CanGoForward;

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
			get => WpfNavigationService.Content;
			set {
				if (WpfNavigationService.Content == value) {
					return;
				}
				WpfNavigationService.Content = value;
			}
		}

		// set navigation service when the first time you get it
		public Navigation::NavigationService WpfNavigationService {
			get {
				if (wpfNavigationService != null) {
					return wpfNavigationService;
				}
				wpfNavigationService = serviceProvider.GetService<MainWindow>().FolderFrame.NavigationService;
				// propagate navigated event
				wpfNavigationService.Navigated += (sender, e) => {
					Navigated?.Invoke(sender, (e.Content as FolderPage)?.Path);
				};
				return wpfNavigationService;
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
			if (!WpfNavigationService.CanGoBack) {
				return;
			}
			WpfNavigationService.GoBack();
		}

		public void GoForward()
		{
			if (!WpfNavigationService.CanGoForward) {
				return;
			}
			WpfNavigationService.GoForward();
		}

		public void GoUp()
		{
			if (!CanGoUp) {
				return;
			}
			var parentPath = GetParentPath();
			var parentFolderPage = serviceProvider.GetService<FolderPage>();
			parentFolderPage.Path = parentPath;
			WpfNavigationService.Navigate(parentFolderPage);
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

		public void Refresh()
		{
			WpfNavigationService.Refresh();
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

		#endregion Private Methods
	}
}