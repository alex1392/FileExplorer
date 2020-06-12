using System;

namespace FileExplorer.Models {

	public abstract class ListItem : Item {

		#region Public Properties
		public string TypeDescription { get; protected set; }

		public DateTimeOffset LastModifiedTime { get; private set; }

		/// <summary>
		/// Property Injection
		/// </summary>
		public override string Path {
			get => base.Path;
			set {
				// can only be injected once
				if (base.Path != null || value == base.Path) {
					return;
				}
				base.Path = value;
				var info = fileProvider.GetFileSystemInfo(base.Path);
				LastModifiedTime = new DateTimeOffset(info.LastWriteTimeUtc);
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public ListItem(IFileProvider fileProvider) : base(fileProvider)
		{
			
		}

		#endregion Public Constructors
	}
}