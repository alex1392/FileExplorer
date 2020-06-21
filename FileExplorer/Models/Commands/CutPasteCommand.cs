using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models
{
	public class CutPasteCommand : PasteCommand
	{
		#region Public Constructors

		public CutPasteCommand(IFileProvider fileProvider, INavigationService navigationService) : base(fileProvider, navigationService)
		{
		}

		#endregion Public Constructors

		#region Public Methods

		public override bool CanExecute(object parameter = null)
		{
			return SourcePaths != null && !string.IsNullOrEmpty(DestPath);
		}

		public override void Execute(object parameter = null)
		{
			if (!CanExecute())
			{
				throw new InvalidOperationException();
			}
			var UnSuccessPaths = new List<string>();
			foreach (var path in SourcePaths)
			{
				var isSuccessful = fileProvider.Move(path, DestPath);
				// record unsuccessful paths
				if (!isSuccessful)
				{
					UnSuccessPaths.Add(path);
				}
			}
			// remove unsuccessful paths
			foreach (var path in UnSuccessPaths)
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
			foreach (var path in SourcePaths)
			{
				var filename = fileProvider.GetFileName(path);
				var sourceFolder = fileProvider.GetParent(path);
				var isSuccessful = fileProvider.Move(Path.Combine(DestPath, filename), sourceFolder);
				// record unsuccessful paths
				if (!isSuccessful)
				{
					UnSuccessPaths.Add(path);
				}
			}
			// remove unsuccessful paths
			foreach (var path in UnSuccessPaths)
			{
				SourcePaths.Remove(path);
			}
			// if there's no any path successfully moved, the execution is not successful
			IsUndoSuccessful = SourcePaths.Count > 0;
			// refresh page if successful
			if (IsUndoSuccessful)
			{
				navigationService.Refresh();
			}
		}

		#endregion Public Methods
	}
}