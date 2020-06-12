using System;

namespace FileExplorer.Models {

	public abstract class ListItem : Item {

		#region Public Properties

		public DateTimeOffset LastModifiedTime { get; set; }

		#endregion Public Properties

		#region Public Constructors

		public ListItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{
			var info = fileProvider.GetFileSystemInfo(path);
			LastModifiedTime = new DateTimeOffset(info.LastWriteTimeUtc);
		}

		#endregion Public Constructors
	}
}