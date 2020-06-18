using FileExplorer.Models;
using GongSolutions.Wpf.DragDrop;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace FileExplorer.ViewModels
{
	public class FileDropHandler : IDropTarget
	{
		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		public FileDropHandler(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}
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
			if (!(dropInfo.DragInfo.DataObject is DataObject data))
			{
				return;
			}
			if (!data.GetDataPresent(DataFormats.FileDrop))
			{
				return;
			}
			var paths = data.GetFileDropList();
			foreach (var path in paths)
			{
				if (dropInfo.TargetItem is ListFolderItemViewModel targetFolderVM)
				{
					fileProvider.Move(path, targetFolderVM.Path);
				}
				else if (dropInfo.TargetItem is ListFileItemViewModel targetFileVM)
				{
					var parentPath = fileProvider.GetParent(targetFileVM.Path);
					fileProvider.Move(path, parentPath);
				}
			}
			navigationService.Refresh();
		}

	}
}