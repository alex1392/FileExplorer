using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Models
{
	public class CreateCommand : IUndoCommand
	{
		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		public event EventHandler CanExecuteChanged;
		public CreateCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}
		public string Path { get; set; }

		public string CreatedPath { get; private set; }

		public bool CanExecute(object parameter)
		{
			return Path != null;
		}

		public void Execute(object parameter)
		{
			var result = fileProvider.Create(Path);
			IsExecutionSuccessful = result != null;
			CreatedPath = result;
			if (IsExecutionSuccessful)
			{
				navigationService.Refresh();
			}

		}

		public void Undo()
		{
			var result = fileProvider.Delete(CreatedPath);
			IsUndoSuccessful = result;
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}
	}
}
