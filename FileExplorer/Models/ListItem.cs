using System;

namespace FileExplorer.Models {
	public abstract class ListItem : Item {
		public DateTimeOffset LastModifiedTime { get; set; }

		public ListItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{
			var info = fileProvider.GetFileSystemInfo(path);
			LastModifiedTime = new DateTimeOffset(info.LastWriteTimeUtc);
		}
	}
}
