using System.Collections.Generic;

namespace FileExplorer.Models
{
	public interface ITreeItem
	{
		#region Public Properties

		List<Item> SubFolders { get; }
		string IconKey { get; }
		string Name { get; }

		#endregion Public Properties
	}
}