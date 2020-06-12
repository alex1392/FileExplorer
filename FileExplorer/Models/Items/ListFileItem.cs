namespace FileExplorer.Models {

	public class ListFileItem : ListItem {

		#region Public Properties

		public long Size { get; set; }

		#endregion Public Properties

		#region Public Constructors

		public ListFileItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{
			var info = fileProvider.GetFileInfo(path);
			Size = info.Length;
		}

		#endregion Public Constructors
	}
}