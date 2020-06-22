using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class GoForwardCommand : ICommand
	{
		#region Private Fields

		private readonly INavigationService navigationService;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Constructors

		public GoForwardCommand(INavigationService navigationService)
		{
			this.navigationService = navigationService;
			navigationService.Navigated += (sender, e) =>
			{
				CanExecuteChanged?.Invoke(this, null);
			};
		}

		#endregion Public Constructors

		#region Public Methods

		public bool CanExecute(object parameter)
		{
			return navigationService.CanGoForward;
		}

		public void Execute(object parameter)
		{
			navigationService.GoForward();
		}

		#endregion Public Methods
	}
}