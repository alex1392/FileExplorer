using FileExplorer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace FileExplorer.Views {
	public class FolderNavigationService : IFolderNavigationService {
		private readonly IServiceProvider serviceProvider;
		private NavigationService navigationService;

		// set navigation service when the first time you get it
		public NavigationService NavigationService => 
			navigationService ?? 
			(navigationService = serviceProvider.GetService<MainWindow>().FolderFrame.NavigationService);

		public FolderNavigationService(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		private void EnsureServiceInjected()
		{
			if (NavigationService == null) {
				throw new InvalidOperationException("Navigation service has not been set.");
			}
		}

		public void GoBack()
		{
			EnsureServiceInjected();
			if (NavigationService.CanGoBack) {
				NavigationService.GoBack();
			}
		}

		public void GoForward()
		{
			EnsureServiceInjected();
			if (NavigationService.CanGoForward) {
				NavigationService.GoForward();
			}
		}

		public void Navigate(string pageKey, object parameter)
		{
			EnsureServiceInjected();
			var page = pageKey switch {
				"FolderPage" => serviceProvider.GetService<FolderPage>(),
				_ => throw new InvalidOperationException("Cannot recognize given pageKey.")
			};
			// check if destination path is exist
			var destPath = parameter.ToString();
			var fileProvider = serviceProvider.GetService<IFileProvider>();
			if (!fileProvider.IsDirectoryExists(destPath)) {
				serviceProvider.GetService<IDialogService>().ShowMessage("Specified folder path does not exist.");
				return;
			}
			page.Path = destPath;
			NavigationService.Navigate(page);
		}
	}
}
