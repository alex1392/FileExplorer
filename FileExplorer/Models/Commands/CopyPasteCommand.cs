using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models
{
	public class CopyPasteCommand : PasteCommand
	{
		#region Private Fields

		private readonly List<string> CopyedPaths = new List<string>();

		#endregion Private Fields



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
			SourcePaths.RemoveAll(path =>
			{
				var filename = Path.GetFileName(path);
				var destPath = fileProvider.Copy(path, Path.Combine(DestPath, filename));
				var successful = !string.IsNullOrEmpty(destPath);
				if (successful)
				{
					CopyedPaths.Add(destPath);
				}
				return !successful;
			});
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
			CopyedPaths.RemoveAll(path =>
			{
				// delete copyed file
				var isSuccessful = fileProvider.Delete(path);
				return !isSuccessful;
			});
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