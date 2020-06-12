using System.Linq;

namespace FileExplorer.Models {

	public class Item {

		#region Protected Fields

		protected IFileProvider fileProvider;

		#endregion Protected Fields

		#region Public Properties

		public string Name { get; private set; }
		public string Path { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public Item(string path, IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
			// fix drive path
			if (path.Last() == ':') {
				path += System.IO.Path.DirectorySeparatorChar;
			}
			var info = fileProvider.GetFileSystemInfo(path);
			Name = info.Name;
			Path = info.FullName;
		}

		#endregion Public Constructors
	}
}