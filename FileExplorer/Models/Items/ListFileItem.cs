using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileExplorer.Models {
	public class ListFileItem : ListItem {
		public long Size { get; set; }
		public string Extension { get; set; }

		public ListFileItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{
			var info = fileProvider.GetFileInfo(path);
			Size = info.Length;
			Extension = info.Extension;
		}
	}
}
