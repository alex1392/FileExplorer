using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Models {
	public class ListFolderItem : ListItem {
		public ListFolderItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{

		}
	}
}
