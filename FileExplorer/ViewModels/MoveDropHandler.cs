using FileExplorer.Models;
using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace FileExplorer.ViewModels
{
	public class MoveDropHandler : IDropTarget
	{
		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;

		public MoveDropHandler(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
		}
		public void DragOver(IDropInfo dropInfo)
		{
			if (dropInfo.TargetItem is ListFileItemViewModel targetFile)
			{
				dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
				dropInfo.Effects = DragDropEffects.Move;
			}
			if (dropInfo.TargetItem is ListFolderItemViewModel targetFolder)
			{
				dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
				dropInfo.Effects = DragDropEffects.Move;
			}
		}

		public void Drop(IDropInfo dropInfo)
		{
			var sourceItemVM = dropInfo.Data as ListItemViewModel;
			if (dropInfo.TargetItem is ListFolderItemViewModel targetFolderVM)
			{
				// move item to target folder
				fileProvider.Move(sourceItemVM.Path, targetFolderVM.Path);
				navigationService.Refresh();
			}
			else if (dropInfo.TargetItem is ListFileItemViewModel targetFileVM)
			{
				var handler = new DefaultDropHandler();
				handler.Drop(dropInfo);
			}
		}

	}
}