namespace FileExplorer.Models {

	public class ListFolderItem : ListItem {

		#region Public Constructors

		public ListFolderItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{
		}

		#endregion Public Constructors
	}
}