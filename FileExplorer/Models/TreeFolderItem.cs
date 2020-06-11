﻿using FileExplorer.DataVirtualization;
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
	}
}
