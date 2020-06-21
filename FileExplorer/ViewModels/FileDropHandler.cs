using GongSolutions.Wpf.DragDrop;

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
			if (!(dropInfo.Data is DataObject data))
			{
				return;
			}
			if (!data.GetDataPresent(DataFormats.FileDrop))
			{
				return;
			}
			var paths = data.GetFileDropList();
			foreach (var path in paths)
			{   // if drop to a folder
				if (dropInfo.TargetItem is ListFolderItemViewModel targetFolderVM)
				{   // move file to the target folder
					FolderPageVM.MoveFile(path, targetFolderVM.Path);
				}
				else // if drop to empty area or file item
				{   // move file to the current folder
					FolderPageVM.MoveFile(path, FolderPageVM.Path);
				}
			}
		}

		#endregion Public Methods
	}
}