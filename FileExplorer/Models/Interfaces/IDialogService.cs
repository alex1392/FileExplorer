using System.Windows;

namespace FileExplorer.Models
{
	public interface IDialogService
	{
		#region Public Methods

		MessageBoxResult ShowDialog(string message, string caption, MessageBoxButton button);

		#endregion Public Methods

		#region Public Methods

		void ShowMessage(string message);
		string ShowFileNameDialog();

		#endregion Public Methods
	}
}