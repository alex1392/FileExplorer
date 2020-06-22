using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models
{
	public class CopyPasteCommand : PasteCommand
	{
		private readonly List<string> CopyedPaths = new List<string>();
		#region Public Properties


		#endregion Public Properties

		#region Public Constructors

		public CopyPasteCommand(IFileProvider fileProvider, INavigationService navigationService) : base(fileProvider, navigationService)
		{
		}

		#endregion Public Constructors

		#region Public Methods

		public override bool CanExecute(object parameter = null)
		{
			return SourcePaths != null && !string.IsNullOrEmpty(DestPath);
		}

		public override void Execute(object parameter)
		{
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}
			var unsuccessPaths = new List<string>();
			foreach (var path in SourcePaths)
			{
				var filename = Path.GetFileName(path);
				var result = fileProvider.Copy(path, Path.Combine(DestPath, filename));
				// record paths
				if (string.IsNullOrEmpty(result))
				{
					unsuccessPaths.Add(path);
				}
				else
				{
					CopyedPaths.Add(result);
				}
			}
			// remove unsuccessful paths
			foreach (var path in unsuccessPaths)
			{
				SourcePaths.Remove(path);
			}
			// if there's no any path successfully moved, the execution is not successful
			IsExecutionSuccessful = SourcePaths.Count > 0;
			// refresh page if successful
			if (IsExecutionSuccessful)
			{
				navigationService.Refresh();
			}
		}

		public override void Undo()
		{
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}
			var UnSuccessPaths = new List<string>();
			foreach (var path in CopyedPaths)
			{
				// delete copyed file
				var isSuccessful = fileProvider.Delete(path);
				// record unsuccessful paths
				if (!isSuccessful)
				{
					UnSuccessPaths.Add(path);
				}
			}
			// remove unsuccessful paths
			foreach (var path in UnSuccessPaths)
			{
				CopyedPaths.Remove(path);
			}
			// if there's no any path successfully moved, the execution is not successful
			IsUndoSuccessful = CopyedPaths.Count > 0;
			// refresh page if successful
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}

		#endregion Public Methods
	}
}