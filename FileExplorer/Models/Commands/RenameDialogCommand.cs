using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileExplorer.Models
{

	public class RenameDialogCommand : ICommand
	{
		private readonly IDialogService dialogService;
		private readonly IServiceProvider serviceProvider;
		private readonly UndoRedoManager undoRedoManager;

		public event EventHandler CanExecuteChanged;
		
		public RenameDialogCommand(IDialogService dialogService, IServiceProvider serviceProvider, UndoRedoManager undoRedoManager)
		{
			this.dialogService = dialogService;
			this.serviceProvider = serviceProvider;
			this.undoRedoManager = undoRedoManager;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var sourcePath = parameter?.ToString();
			if (sourcePath == null)
			{
				return;
			}
			var filename = dialogService.ShowFileNameDialog();
			if (filename == null)
			{
				return;
			}
			var command = serviceProvider.GetService<RenameFileCommand>();
			command.SourcePath = sourcePath;
			command.ChangedName = filename;
			undoRedoManager.Execute(command);
		}
	}
}
