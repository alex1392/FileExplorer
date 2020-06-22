using System;

namespace FileExplorer.Models
{
	public class CreateCommand : IUndoCommand
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
		public string Path { get; set; }

		public string CreatedPath { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public CreateCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}

		#endregion Public Constructors

		#region Public Methods

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

		#endregion Public Methods
	}
}