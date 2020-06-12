using FileExplorer.DataVirtualization;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.ViewModels {
	public class FolderChildrenProvider : IItemsProvider<Item> {
		private string[] folderPaths;
		private string[] filePaths;
		private bool IsChildrenPathsLoaded;
		private string path;
		private readonly IFileProvider fileProvider;

		public string Path {
			get => path;
			set {
				// can only be set once
				if (path != null || path == value) {
					return;
				}
				path = value;
			}
		}

		public FolderChildrenProvider(IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
		}

		private void LoadChildrenPaths()
		{
			if (path == null) {
				throw new InvalidOperationException("Folder Path has not been set.");
			}
			if (IsChildrenPathsLoaded) {
				return;
			}
			(folderPaths, filePaths) = fileProvider.GetChildren(path);
			IsChildrenPathsLoaded = true;
		}

		public int FetchCount()
		{
			LoadChildrenPaths();
			return folderPaths?.Length + filePaths?.Length ?? 0;
		}

		public IList<Item> FetchRange(int startIndex, int count)
		{
			LoadChildrenPaths();
			startIndex = Math.Max(0, startIndex);
			return folderPaths.Skip(startIndex)
			   .Take(count)
			   .Select(path => new ListFolderItem(path, fileProvider) as Item)
			   .Concat(filePaths.Skip(Math.Max(startIndex - folderPaths.Length, 0))
					   .Take(count - Math.Max(folderPaths.Length - startIndex, 0))
					   .Select(path => new ListFileItem(path, fileProvider)))
			   .ToList();
		}
	}
}
