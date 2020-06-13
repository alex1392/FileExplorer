namespace FileExplorer.Models {

	public class ListFolderItem : ListItem {

		#region Private Fields

		private readonly ITypeDescriptionProvider typeDescriptionProvider;

		#endregion Private Fields

		#region Public Properties

		public override string Path {
			get => base.Path;
			set {
				if (base.Path != null || value == base.Path) {
					return;
				}
				base.Path = value;
				TypeDescription = typeDescriptionProvider.GetFolderTypeDescription(base.Path);
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public ListFolderItem(IFileProvider fileProvider, ITypeDescriptionProvider typeDescriptionProvider) : base(fileProvider)
		{
			this.typeDescriptionProvider = typeDescriptionProvider;
		}

		#endregion Public Constructors
	}
}