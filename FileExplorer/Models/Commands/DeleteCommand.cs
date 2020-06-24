using System;
using System.Collections.Generic;

namespace FileExplorer.Models
{
	public class DeleteCommand : IUndoCommand
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
		public List<string> Paths { get; set; }

		#endregion Public Properties

		#region Public Constructors

		public DeleteCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}

		#endregion Public Constructors

		#region Public Methods

		public bool CanExecute(object parameter = null)
		{
			return Paths != null;
		}

		public void Execute(object parameter)
		{
			// delete every path in Paths to bin, if is not successful, remove the path from Paths.
			Paths.RemoveAll(path => !fileProvider.DeleteToBin(path));
			// if there's no any path successfully moved, the execution is not successful
			IsExecutionSuccessful = Paths.Count > 0;
			// refresh page if successful
			if (IsExecutionSuccessful)
			{
				navigationService.Refresh();
			}
		}

		public void Undo()
		{
			// restore every path in Paths, if is not successful, remove the path from Paths
			Paths.RemoveAll(path => !fileProvider.RestoreFromBin(path));
			// if there's no any path successfully moved, the execution is not successful
			IsUndoSuccessful = Paths.Count > 0;
			// refresh page if successful
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}

		#endregion Public Methods
	}
}