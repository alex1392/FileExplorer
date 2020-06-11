using FileExplorer.DataVirtualization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileExplorer.Models {
	public class TreeFolderItem : Item {
		private string[] folderPaths;
		private bool HasExpanded = false;
		public List<Item> SubFolders { get; set; } = new List<Item>
		{
			null
		};

		/// <summary>
		/// Root constructor
		/// </summary>
		public TreeFolderItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{

		}

		/// <summary>
		/// Root Constructor with icon
		/// </summary>
		public TreeFolderItem(string path, IFileProvider fileProvider, ImageSource icon) : this(path, fileProvider)
		{
			Icon = icon;
		}
		/// <summary>
		/// Child constructor
		/// </summary>
		public TreeFolderItem(string path, TreeFolderItem parent) : base(path, parent)
		{
		}

		public void LoadSubFolders()
		{
			if (HasExpanded) {
				return;
			}
			SubFolders.Clear();
			folderPaths = fileProvider.GetDirectories(Path);
			SubFolders.AddRange(folderPaths.Select(path => new TreeFolderItem(path, this)));
			HasExpanded = true;
		}

		//private void LoadChildrenPaths()
		//{
		//	if (IsChildrenPathsLoaded) {
		//		return;
		//	}
		//	folderPaths = fileProvider.GetDirectories(Path);
		//	filePaths = fileProvider.GetFiles(Path);
		//	IsChildrenPathsLoaded = true;
		//}

		//public int FetchCount()
		//{
		//	LoadChildrenPaths();
		//	return folderPaths?.Length + filePaths?.Length ?? 0;
		//}

		//public IList<Item> FetchRange(int startIndex, int count)
		//{
		//	LoadChildrenPaths();

		//	startIndex = Math.Max(0, startIndex);
		//	return folderPaths.Skip(startIndex)
		//	   .Take(count)
		//	   .Select(path => new TreeFolderItem(path, this) as Item)
		//	   .Concat(filePaths.Skip(Math.Max(startIndex - folderPaths.Length, 0))
		//			   .Take(count - Math.Max(folderPaths.Length - startIndex, 0))
		//			   .Select(path => new FileItem(path, this)))
		//	   .ToList();
		//}
	}
}
