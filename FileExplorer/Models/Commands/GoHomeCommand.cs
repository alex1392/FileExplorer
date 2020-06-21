using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class GoHomeCommand : ICommand
	{
		private readonly INavigationService navigationService;
		private readonly TreePageItem homePage;

		public GoHomeCommand(INavigationService navigationService, TreePageItem homePage)
		{
			this.navigationService = navigationService;
			this.homePage = homePage;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			navigationService.Navigate(homePage.Uri);
		}
	}
}