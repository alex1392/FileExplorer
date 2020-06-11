using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileExplorer.Models {
	public abstract class Item {
		protected IFileProvider fileProvider;
		public string Path { get; private set; }
		public string Name { get; private set; }
		public DateTimeOffset LastModifiedTime { get; set; }
		public TreeFolderItem Parent { get; private set; }
		/// <summary>
		/// Allow customized icon
		/// </summary>
		public virtual ImageSource Icon { get; protected set; }

		public Item(string path, IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
			var info = fileProvider.GetFileSystemInfo(path);
			Name = info.Name;
			Path = info.FullName;
			LastModifiedTime = new DateTimeOffset(info.LastWriteTimeUtc);
		}

		public Item(string path, TreeFolderItem parent) : this(path, parent.fileProvider)
		{
			Parent = parent;
		}
	}
}
