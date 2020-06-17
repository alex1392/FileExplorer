using System.Collections.Generic;

namespace FileExplorer.Models
{
	public interface ITreeItem 
	{
		List<Item> SubFolders { get; }
		string IconKey { get; }
		string Name { get; }
	}
}