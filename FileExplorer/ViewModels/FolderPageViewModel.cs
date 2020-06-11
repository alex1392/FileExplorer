using FileExplorer.DataVirtualization;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels {
	public class FolderChildrenProvider : IItemsProvider<Item> {
		private string[] folderPaths;
		private string[] filePaths;
		private bool IsChildrenPathsLoaded;
		private string path;
		private readonly IFileProvider fileProvider;

		public string Path {
			get => path;
			set {
				// can only be set once
				if (path != null || path == value) {
					return;
				}
				path = value;
			}
		}

		public FolderChildrenProvider(IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
		}

		private void LoadChildrenPaths()
		{
			if (path == null) {
				throw new InvalidOperationException("Folder Path has not been set.");
			}
			if (IsChildrenPathsLoaded) {
				return;
			}
			folderPaths = fileProvider.GetDirectories(Path);
			filePaths = fileProvider.GetFiles(Path);
			IsChildrenPathsLoaded = true;
		}

		public int FetchCount()
		{
			LoadChildrenPaths();
			return folderPaths?.Length + filePaths?.Length ?? 0;
		}

		public IList<Item> FetchRange(int startIndex, int count)
		{
			LoadChildrenPaths();
			startIndex = Math.Max(0, startIndex);
			return folderPaths.Skip(startIndex)
			   .Take(count)
			   .Select(path => new ListFolderItem(path, fileProvider) as Item)
			   .Concat(filePaths.Skip(Math.Max(startIndex - folderPaths.Length, 0))
					   .Take(count - Math.Max(folderPaths.Length - startIndex, 0))
					   .Select(path => new ListFileItem(path, fileProvider)))
			   .ToList();
		}
	}

	public class FolderPageViewModel : INotifyPropertyChanged {
		private readonly FolderChildrenProvider folderChildrenProvider;
		private string path;

		public event PropertyChangedEventHandler PropertyChanged;

		public VirtualizingCollection<Item> ListItems { get; private set; }

		public IEnumerable<string> PathItems { get; private set; }

		/// <summary>
		/// Property injection
		/// </summary>
		public string Path {
			get => path;
			internal set {
				// can only be set once
				if (path != null || path == value) {
					return;
				}
				path = value;

				folderChildrenProvider.Path = path;
				ListItems = new VirtualizingCollection<Item>(folderChildrenProvider, 20);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListItems)));

				PathItems = path.Split(System.IO.Path.DirectorySeparatorChar).Where(s => !string.IsNullOrEmpty(s));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathItems)));
			}
		}

		/// <summary>
		/// for xaml designer
		/// </summary>
		public FolderPageViewModel()
		{

		}
		public FolderPageViewModel(FolderChildrenProvider folderChildrenProvider)
		{
			this.folderChildrenProvider = folderChildrenProvider;
		}


	}
}
