namespace FileExplorer.Models
{
	public class ListFileItem : ListItem
	{
		#region Private Fields

		private readonly ITypeDescriptionProvider typeDescriptionProvider;

		#endregion Private Fields

		#region Public Properties

		public override string Path
		{
			get => base.Path;
			set
			{
				if (base.Path != null || value == base.Path)
				{
					return;
				}
				base.Path = value;
				var info = fileProvider.GetFileInfo(base.Path);
				Size = info.Length;
				TypeDescription = typeDescriptionProvider.GetFileTypeDescription(base.Path);
			}
		}

		public long Size { get; set; }

		#endregion Public Properties

		#region Public Constructors

		public ListFileItem(IFileProvider fileProvider, ITypeDescriptionProvider typeDescriptionProvider) : base(fileProvider)
		{
			this.typeDescriptionProvider = typeDescriptionProvider;
		}

		#endregion Public Constructors
	}
}