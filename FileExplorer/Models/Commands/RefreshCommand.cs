using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	internal class RefreshCommand : ICommand
	{
		#region Private Fields

		private readonly INavigationService navigationService;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Constructors

		public RefreshCommand(INavigationService navigationService)
		{
			this.navigationService = navigationService;
		}

		#endregion Public Constructors

		#region Public Methods

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			navigationService.Refresh();
		}

		#endregion Public Methods
	}
}