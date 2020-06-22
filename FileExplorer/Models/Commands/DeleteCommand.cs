using System;
using System.Collections.Generic;

namespace FileExplorer.Models
{
	public class DeleteCommand : IUndoCommand
	{
		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		public event EventHandler CanExecuteChanged;

		public DeleteCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}

		public List<string> Paths { get; set; }
		public bool CanExecute(object parameter = null)
		{
			return Paths != null;
		}

		public void Execute(object parameter)
		{
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}
			var unsuccessPaths = new List<string>();
			foreach (var path in Paths)
			{
				var isSuccessful = fileProvider.DeleteToBin(path);
				// record paths
				if (!isSuccessful)
				{
					unsuccessPaths.Add(path);
				}
			}
			// remove unsuccessful paths
			foreach (var path in unsuccessPaths)
			{
				Paths.Remove(path);
			}
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
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}
			var UnSuccessPaths = new List<string>();
			foreach (var path in Paths)
			{
				// delete copyed file
				var isSuccessful = fileProvider.RestoreFromBin(path);
				// record unsuccessful paths
				if (!isSuccessful)
				{
					UnSuccessPaths.Add(path);
				}
			}
			// remove unsuccessful paths
			foreach (var path in UnSuccessPaths)
			{
				Paths.Remove(path);
			}
			// if there's no any path successfully moved, the execution is not successful
			IsUndoSuccessful = Paths.Count > 0;
			// refresh page if successful
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}
	}
}