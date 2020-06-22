using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class GoHomeCommand : ICommand
	{
		#region Private Fields

		private readonly INavigationService navigationService;

		public TreePageItem HomePage { get; set; }

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Constructors

		public GoHomeCommand(INavigationService navigationService)
		{
			this.navigationService = navigationService;
		}

		#endregion Public Constructors

		#region Public Methods

		public bool CanExecute(object parameter)
		{
			return HomePage != null;
		}

		public void Execute(object parameter)
		{
			navigationService.Navigate(HomePage.Uri);
		}

		#endregion Public Methods
	}
}