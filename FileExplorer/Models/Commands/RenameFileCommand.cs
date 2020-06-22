using System;
using System.IO;

namespace FileExplorer.Models
{
	public class RenameFileCommand : IUndoCommand
	{
		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		public string SourcePath { get; set; }

		public string ChangedName { get; set; }
		public string DestPath { get; private set; }

		public event EventHandler CanExecuteChanged;

		public RenameFileCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}

		public bool CanExecute(object parameter)
		{
			return !string.IsNullOrEmpty(SourcePath) && 
				!string.IsNullOrEmpty(ChangedName);
		}

		public void Execute(object parameter)
		{
			var ext = Path.GetExtension(SourcePath);
			var folderPath = Path.GetDirectoryName(SourcePath);
			var result = fileProvider.Move(SourcePath, Path.Combine(folderPath, ChangedName + ext));
			IsExecutionSuccessful = result != null;
			if (IsExecutionSuccessful)
			{
				DestPath = result;
				navigationService.Refresh();
			}
		}

		public void Undo()
		{
			var result = fileProvider.Move(DestPath, SourcePath);
			IsUndoSuccessful = result != null;
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}
	}
}
