using System;
using System.IO;

namespace FileExplorer.Models
{
	public class MoveFileCommand : IUndoCommand
	{
		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Properties

		public string SourcePath { get; set; }
		public string DestPath { get; set; }

		public bool IsExecutionSuccessful { get; private set; }
		public bool IsUndoSuccessful { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public MoveFileCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}

		#endregion Public Constructors

		#region Public Methods

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

		#endregion Public Methods
	}
}