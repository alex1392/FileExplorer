using GongSolutions.Wpf.DragDrop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FileExplorer.ViewModels
{
	public class FileDropHandler : IDropTarget
	{
		#region Public Properties

		public FolderPageViewModel FolderPageVM { get; set; }

		#endregion Public Properties

		#region Public Methods

		public void DragOver(IDropInfo dropInfo)
		{
			if (dropInfo.TargetItem is ListFolderItemViewModel targetFolder)
			{
				dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
			}
			dropInfo.Effects = DragDropEffects.Move;
		}

		public void Drop(IDropInfo dropInfo)
		{
			IEnumerable<string> paths;
			if (dropInfo.Data is DataObject data &&
				data.GetDataPresent(DataFormats.FileDrop))
			{
				paths = data.GetFileDropList().OfType<string>();
			}
			else if (dropInfo.Data is string[] strArray)
			{
				paths = strArray;
			}
			else
			{
				throw new InvalidOperationException();
			}
			// if drop to a folder
			if (dropInfo.TargetItem is ListFolderItemViewModel targetFolderVM)
			{   // move file to the target folder
				FolderPageVM.MoveFile(paths, targetFolderVM.Path);
			}
			else // if drop to empty area or file item
			{   // move file to the current folder
				FolderPageVM.MoveFile(paths, FolderPageVM.Path);
			}
		}

		#endregion Public Methods
	}
}