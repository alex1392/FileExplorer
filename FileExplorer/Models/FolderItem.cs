using FileExplorer.DataVirtualization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileExplorer.Models {
	public class FolderItem : Item, IItemsProvider<Item> {
		private string[] folderPaths;
		private string[] filePaths;
		private bool IsSubFoldersLoaded = false;
		private bool IsChildrenPathsLoaded;
		public VirtualizingCollection<Item> Children { get; private set; }
		public List<Item> SubFolders { get; set; } = new List<Item>
		{
			null
		};

		/// <summary>
		/// Root constructor
		/// </summary>
		public FolderItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{
			Setup();
		}

		/// <summary>
		/// Root Constructor with icon
		/// </summary>
		public FolderItem(string path, IFileProvider fileProvider, ImageSource icon) : this(path, fileProvider)
		{
			Icon = icon;
		}
		/// <summary>
		/// Child constructor
		/// </summary>
		public FolderItem(string path, FolderItem parent) : base(path, parent)
		{
			Setup();
		}

		private void Setup()
		{
			Children = new VirtualizingCollection<Item>(this, 10);
		}

		public void LoadSubFolders()
		{
			if (IsSubFoldersLoaded) {
				return;
			}
			SubFolders.Clear();
			SubFolders.AddRange(Children.TakeWhile(item => item is FolderItem));
			IsSubFoldersLoaded = true;
		}

		private void LoadChildrenPaths()
		{
			if (IsChildrenPathsLoaded) {
				return;
			}
			folderPaths = fileProvider.GetDirectories(Path);
			filePaths = fileProvider.GetFiles(Path);
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
			   .Select(path => new FolderItem(path, this) as Item)
			   .Concat(filePaths.Skip(Math.Max(startIndex - folderPaths.Length, 0))
					   .Take(count - Math.Max(folderPaths.Length - startIndex, 0))
					   .Select(path => new FileItem(path, this)))
			   .ToList();
		}
	}
}
