
using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models
{
	public class CopyPasteCommand : PasteCommand
	{
		public List<string> CopyedPaths { get; set; } = new List<string>();
		public CopyPasteCommand(IFileProvider fileProvider, INavigationService navigationService) : base(fileProvider, navigationService)
		{
		}

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
				var filename = fileProvider.GetFileName(path);
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
	}

	
}