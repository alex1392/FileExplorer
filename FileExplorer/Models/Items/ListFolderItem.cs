using FileExplorer.Converters;

namespace FileExplorer.Models {

	public class ListFolderItem : ListItem {
		private readonly ITypeDescriptionProvider typeDescriptionProvider;

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

		#region Public Constructors

		public ListFolderItem(IFileProvider fileProvider, ITypeDescriptionProvider typeDescriptionProvider) : base(fileProvider)
		{
			this.typeDescriptionProvider = typeDescriptionProvider;
		}

		#endregion Public Constructors
	}
}