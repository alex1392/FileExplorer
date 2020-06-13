using FileExplorer.DataVirtualization;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Models {

	public class FolderChildrenProvider : IItemsProvider<ListItem> {

		#region Private Fields

		private readonly IFileProvider fileProvider;
		private readonly IServiceProvider serviceProvider;
		private string[] filePaths;
		private string[] folderPaths;
		private bool IsChildrenPathsLoaded;
		private string path;

		#endregion Private Fields

		#region Public Properties

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

		#endregion Public Properties

		#region Public Constructors

		public FolderChildrenProvider(IFileProvider fileProvider, IServiceProvider serviceProvider)
		{
			this.fileProvider = fileProvider;
			this.serviceProvider = serviceProvider;
		}

		#endregion Public Constructors

		#region Public Methods

		public int FetchCount()
		{
			LoadChildrenPaths();
			return folderPaths?.Length + filePaths?.Length ?? 0;
		}

		public IList<ListItem> FetchRange(int startIndex, int count)
		{
			LoadChildrenPaths();
			startIndex = Math.Max(0, startIndex);
			return folderPaths.Skip(startIndex)
			   .Take(count)
			   .Select(path => {
				   var folderItem = serviceProvider.GetService<ListFolderItem>();
				   folderItem.Path = path;
				   return folderItem;
			   })
			   .Cast<ListItem>()
			   .Concat(filePaths.Skip(Math.Max(startIndex - folderPaths.Length, 0))
					   .Take(count - Math.Max(folderPaths.Length - startIndex, 0))
					   .Select(path => {
						   var fileItem = serviceProvider.GetService<ListFileItem>();
						   fileItem.Path = path;
						   return fileItem;
					   }))
			   .ToList();
		}

		#endregion Public Methods

		#region Private Methods

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

		#endregion Private Methods
	}
}