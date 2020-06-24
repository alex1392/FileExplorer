using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class RenameDialogCommand : ICommand
	{
		#region Private Fields

		private readonly IDialogService dialogService;
		private readonly IServiceProvider serviceProvider;
		private readonly UndoRedoManager undoRedoManager;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Constructors

		public RenameDialogCommand(IDialogService dialogService, IServiceProvider serviceProvider, UndoRedoManager undoRedoManager)
		{
			this.dialogService = dialogService;
			this.serviceProvider = serviceProvider;
			this.undoRedoManager = undoRedoManager;
		}

		#endregion Public Constructors

		#region Public Methods

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
			var (result, filename) = dialogService.ShowFileNameDialog();
			if (!result)
			{
				return;
			}
			if (string.IsNullOrEmpty(filename))
			{
				dialogService.ShowMessage("File Name shouldn't be empty.");
				return;
			}
			var command = serviceProvider.GetService<RenameFileCommand>();
			command.SourcePath = sourcePath;
			command.ChangedName = filename;
			undoRedoManager.Execute(command);
		}

		#endregion Public Methods
	}
}