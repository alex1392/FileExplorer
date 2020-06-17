
using System;
using System.Collections.Generic;

namespace FileExplorer.Models
{
	public class TreePageItem : ITreeItem
	{
		private readonly IFileProvider fileProvider;
		private Uri uri;

		public List<Item> SubFolders => new List<Item>();

		public string IconKey { get; set; }

		public string Name { get; private set; }
		/// <summary>
		/// property injection
		/// </summary>
		public Uri Uri
		{
			get => uri; set
			{
				if (uri != null || value == uri)
				{
					return;
				}
				uri = value;
				Name = fileProvider.GetFileNameWithoutExtension(uri.ToString());
			}
		}

		public TreePageItem(IFileProvider fileProvider)
		{
			this.fileProvider = fileProvider;
		}
	}
}