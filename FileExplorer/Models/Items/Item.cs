using System.Linq;

namespace FileExplorer.Models {

	public class Item {

		#region Protected Fields

		protected IFileProvider fileProvider;

		#endregion Protected Fields

		#region Private Fields

		private string path;

		#endregion Private Fields

		#region Public Properties

		public string Name { get; private set; }

		/// <summary>
		/// Property Injection
		/// </summary>
		public virtual string Path {
			get => path;
			set {
				// can only be set once
				if (path != null || value == path) {
					return;
				}
				path = value;
				// fix drive path
				if (path.Last() == ':') {
					path += System.IO.Path.DirectorySeparatorChar;
				}
				var info = fileProvider.GetFileSystemInfo(path);
				Name = info.Name;
				path = info.FullName;
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public Item(IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
		}

		#endregion Public Constructors
	}
}