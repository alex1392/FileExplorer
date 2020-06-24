using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models
{
	public class CutPasteCommand : PasteCommand
	{
		#region Private Fields

		private readonly List<(string sourcePath, string destPath)> MovedPaths = new List<(string, string)>();

		public CutPasteCommand(IFileProvider fileProvider, INavigationService navigationService) : base(fileProvider, navigationService)
		{
		}

		#endregion Private Fields

		#region Public Methods

		public override bool CanExecute(object parameter = null)
		{
			return SourcePaths != null &&
				!string.IsNullOrEmpty(DestPath);
		}

		public override void Execute(object parameter = null)
		{
			SourcePaths.RemoveAll(sourcePath =>
			{
				var filename = Path.GetFileName(sourcePath);
				var destPath = fileProvider.Move(sourcePath, Path.Combine(DestPath, filename));
				var successful = !string.IsNullOrEmpty(destPath);
				if (successful)
				{
					MovedPaths.Add((sourcePath, destPath));
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
			MovedPaths.RemoveAll(path =>
			{
				var (sourcePath, destPath) = path;
				var successful = fileProvider.Move(destPath, sourcePath) != null;
				return !successful;
			});
			// if there's no any path successfully moved, the execution is not successful
			IsUndoSuccessful = MovedPaths.Count > 0;
			// refresh page if successful
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}

		#endregion Public Methods
	}
}