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

		public string ShowFileNameDialog()
		{
			var dialog = new FileNameInputWindow
			{
				Owner = App.Current.MainWindow
			};
			if (dialog.ShowDialog() ?? false)
			{
				return dialog.fileNameTextBox.Text;
			}
			else
			{
				return null;
			}
		}

		#endregion Public Methods
	}
}