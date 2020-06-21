using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class GoHomeCommand : ICommand
	{
		#region Private Fields

		private readonly INavigationService navigationService;
		private readonly TreePageItem homePage;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Constructors

		public GoHomeCommand(INavigationService navigationService, TreePageItem homePage)
		{
			this.navigationService = navigationService;
			this.homePage = homePage;
		}

		#endregion Public Constructors

		#region Public Methods

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			navigationService.Navigate(homePage.Uri);
		}

		#endregion Public Methods
	}
}