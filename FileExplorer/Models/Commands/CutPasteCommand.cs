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
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}

			var unsuccessPaths = new List<string>();
			foreach (var sourcePath in SourcePaths)
			{
				var filename = Path.GetFileName(sourcePath);
				var destPath = fileProvider.Move(sourcePath, Path.Combine(DestPath, filename));
				// record paths
				if (string.IsNullOrEmpty(destPath))
				{
					unsuccessPaths.Add(sourcePath);
				}
				else
				{
					MovedPaths.Add((sourcePath, destPath));
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
			var unsuccessPaths = new List<(string, string)>();
			foreach (var (sourcePath, destPath) in MovedPaths)
			{
				var result = fileProvider.Move(destPath, sourcePath);
				// record unsuccessful paths
				if (result == null)
				{
					unsuccessPaths.Add((sourcePath, destPath));
				}
			}
			// remove unsuccessful paths
			foreach (var path in unsuccessPaths)
			{
				MovedPaths.Remove(path);
			}
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