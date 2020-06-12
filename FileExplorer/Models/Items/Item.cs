using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileExplorer.Models {
	public class Item {
		protected IFileProvider fileProvider;
		public string Path { get; private set; }
		public string Name { get; private set; }
		
		public Item(string path, IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
			// fix drive path
			if (path.Last() == ':') {
				path += System.IO.Path.DirectorySeparatorChar;
			}
			var info = fileProvider.GetFileSystemInfo(path);
			Name = info.Name;
			Path = info.FullName;
		}
	}
}
