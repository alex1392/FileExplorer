
using System;
using System.IO;

namespace FileExplorer.Models
{
	public class MoveFileCommand : IUndoCommand
	{
		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		public string SourcePath { get; set; }
		public string DestPath { get; set; }

		public bool IsExecutionSuccessful { get; private set; }
		public bool IsUndoSuccessful { get; private set; }

		public MoveFileCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter = null)
		{
			return !string.IsNullOrEmpty(SourcePath) &&
				!string.IsNullOrEmpty(DestPath);
		}

		public void Execute(object parameter = null)
		{
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}
			IsExecutionSuccessful = fileProvider.Move(SourcePath, DestPath);
			if (IsExecutionSuccessful)
			{
				navigationService.Refresh();
			}
		}

		public void Undo()
		{
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}
			var filename = fileProvider.GetFileName(SourcePath);
			var folderPath = fileProvider.GetParent(SourcePath);
			IsUndoSuccessful = fileProvider.Move(Path.Combine(DestPath, filename), folderPath);
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}
	}
}