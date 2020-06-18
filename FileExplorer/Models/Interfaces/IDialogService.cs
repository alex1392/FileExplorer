using System.Windows;

namespace FileExplorer.Models
{
	public interface IDialogService
	{
		MessageBoxResult ShowDialog(string message, string caption, MessageBoxButton button);
		#region Public Methods

		void ShowMessage(string message);

		#endregion Public Methods
	}
}