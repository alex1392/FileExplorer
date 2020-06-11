using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileExplorer.Models {
	class FileItem : Item {
		public long Size { get; set; }
		public string Extension { get; set; }
		/// <summary>
		/// Child constructor
		/// </summary>
		public FileItem(string path, TreeFolderItem parent) : base(path, parent)
		{
			var info = fileProvider.GetFileInfo(path);
			Size = info.Length;
			Extension = info.Extension;
		}


	}
}
