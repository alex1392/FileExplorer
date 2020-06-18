using FileExplorer.Models;

using System.Windows;

namespace FileExplorer.Views.Services
{
	public class DialogService : IDialogService
	{
		#region Public Methods

		public void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}

		public MessageBoxResult ShowDialog(string message, string caption, MessageBoxButton button)
		{
			return MessageBox.Show(message, caption, button);
		}

		#endregion Public Methods
	}
}