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

		public (bool, string) ShowFileNameDialog()
		{
			var dialog = new FileNameInputWindow
			{
				Owner = App.Current.MainWindow
			};
			return (dialog.ShowDialog() ?? false, dialog.fileNameTextBox.Text);
		}

		#endregion Public Methods
	}
}