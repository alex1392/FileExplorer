using FileExplorer.Models;

using System.Windows;

namespace FileExplorer.Views {

	internal class DialogService : IDialogService {

		#region Public Methods

		public void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}

		#endregion Public Methods
	}
}