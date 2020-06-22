using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class ShowRenameDialogCommand : ICommand
	{
		private readonly IDialogService dialogService;

		public event EventHandler CanExecuteChanged;
		
		public ShowRenameDialogCommand(IDialogService dialogService)
		{
			this.dialogService = dialogService;
		}
		public void OnCanExecuteChanged()
		{
			//CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var filename = dialogService.ShowFileNameDialog();
			if (filename == null)
			{
				return;
			}

		}
	}
}
