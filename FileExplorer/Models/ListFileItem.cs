﻿using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileExplorer.Models {
	class ListFileItem : Item {
		public long Size { get; set; }
		public string Extension { get; set; }

		public ListFileItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{
			var info = fileProvider.GetFileInfo(path);
			Size = info.Length;
			Extension = info.Extension;
		}
		/// <summary>
		/// Child constructor
		/// </summary>
		public ListFileItem(string path, TreeFolderItem parent) : base(path, parent)
		{
			var info = fileProvider.GetFileInfo(path);
			Size = info.Length;
			Extension = info.Extension;
		}


	}
}