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

		(bool, string) ShowFileNameDialog();

		#endregion Public Methods
	}
}