using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Models {

	public class TreeFolderItem : Item {

		#region Private Fields

		private readonly IServiceProvider serviceProvider;
		private string[] folderPaths;
		private bool HasExpanded = false;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Specify custom icon key.
		/// </summary>
		public string IconKey { get; set; }

		public List<Item> SubFolders { get; set; } = new List<Item>
		{
			null
		};

		#endregion Public Properties

		#region Public Constructors

		public TreeFolderItem(IFileProvider fileProvider, IServiceProvider serviceProvider) : base(fileProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		#endregion Public Constructors

		#region Public Methods

		public void LoadSubFolders()
		{
			if (HasExpanded) {
				return;
			}
			SubFolders.Clear();
			folderPaths = fileProvider.GetDirectories(Path);
			SubFolders.AddRange(folderPaths.Select(path => {
				var item = serviceProvider.GetService<TreeFolderItem>();
				item.Path = path;
				return item;
			}));
			HasExpanded = true;
		}

		#endregion Public Methods
	}
}