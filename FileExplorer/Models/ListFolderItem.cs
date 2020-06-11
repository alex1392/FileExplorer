using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Models {
	public class ListFolderItem : Item {
		public ListFolderItem(string path, IFileProvider fileProvider) : base(path, fileProvider)
		{

		}
	}
}
