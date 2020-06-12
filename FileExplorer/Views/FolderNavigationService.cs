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
		/// <summary>
		/// TODO: change to (string pageKey, object parameter)
		/// </summary>
		/// <param name="key"></param>
		public void Navigate(string key)
		{
			EnsureServiceInjected();
			var page = serviceProvider.GetService<FolderPage>();
			page.Path = key;
			NavigationService.Navigate(page);
		}
	}
}
