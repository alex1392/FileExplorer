using FileExplorer.Converters;

namespace FileExplorer.Models {

	public class ListFileItem : ListItem {
		private readonly ITypeDescriptionProvider typeDescriptionProvider;

		#region Public Properties

		public long Size { get; set; }

		public override string Path {
			get => base.Path;
			set {
				if (base.Path != null || value == base.Path) {
					return;
				}
				base.Path = value;
				var info = fileProvider.GetFileInfo(base.Path);
				Size = info.Length;
				TypeDescription = typeDescriptionProvider.GetFileTypeDescription(base.Path);
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public ListFileItem(IFileProvider fileProvider, ITypeDescriptionProvider typeDescriptionProvider) : base(fileProvider)
		{
			this.typeDescriptionProvider = typeDescriptionProvider;
		}

		#endregion Public Constructors
	}
}