using System;
using System.IO;

namespace FileExplorer.Models
{
	public class RenameFileCommand : IUndoCommand
	{
		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Properties

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		public string SourcePath { get; set; }

		public string ChangedName { get; set; }
		public string DestPath { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public RenameFileCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}

		#endregion Public Constructors

		#region Public Methods

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

		#endregion Public Methods
	}
}