using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace FileExplorer.ViewModels
{
	public class FileDragHandler : IDragSource
	{
		private bool alreadyDropped = false;
		public bool CanStartDrag(IDragInfo dragInfo)
		{
			return true;
		}

		public void DragCancelled()
		{

		}

		public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
		{
			if (alreadyDropped || dragInfo == null)
			{
				return;
			}

			// the drag operation has finished on another app
			if (operationResult != DragDropEffects.None)
			{
				if (operationResult.HasFlag(DragDropEffects.Move))
				{
					var sourceList = dragInfo.SourceCollection.TryGetList();
					var items = dragInfo.SourceItems.OfType<object>().ToList();
					foreach (var item in items)
					{
						sourceList?.Remove(item);
					}
					alreadyDropped = true;
				}
			}
		}

		public void Dropped(IDropInfo dropInfo)
		{
			alreadyDropped = true;
		}

		public void StartDrag(IDragInfo dragInfo)
		{
			alreadyDropped = false;
			var paths = dragInfo.SourceItems.OfType<ListItemViewModel>().Select(vm => vm.Path).ToArray();
			var pathCollection = new StringCollection();
			pathCollection.AddRange(paths);
			var dataObject = new DataObject();
			dataObject.SetFileDropList(pathCollection);
			// need to set dataobject directly in order to use SetFileDropList() method
			dragInfo.DataObject = dataObject;
			dragInfo.DataFormat = DataFormats.GetDataFormat(DataFormats.FileDrop);
			dragInfo.Effects = dragInfo.DataObject != null
				? DragDropEffects.Move
				: DragDropEffects.None;
		}

		public bool TryCatchOccurredException(Exception exception)
		{
			throw exception;
		}
	}
}