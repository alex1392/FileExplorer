using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace FileExplorer.Models {

	public class TreeFolderItem : Item {

		#region Private Fields

		private string[] folderPaths;
		private bool HasExpanded = false;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Allow customized icon
		/// </summary>
		public virtual ImageSource Icon { get; protected set; }

		public List<Item> SubFolders { get; set; } = new List<Item>
		{
			null
		};

		#endregion Public Properties

		#region Public Constructors

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

		#endregion Public Constructors

		#region Public Methods

		public void LoadSubFolders()
		{
			if (HasExpanded) {
				return;
			}
			SubFolders.Clear();
			folderPaths = fileProvider.GetDirectories(Path);
			SubFolders.AddRange(folderPaths.Select(path => new TreeFolderItem(path, fileProvider)));
			HasExpanded = true;
		}

		#endregion Public Methods
	}
}