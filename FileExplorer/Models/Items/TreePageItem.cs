using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models
{
	public class TreePageItem : ITreeItem
	{
		#region Private Fields

		private Uri uri;

		#endregion Private Fields

		#region Public Properties

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
				Name = Path.GetFileNameWithoutExtension(uri.ToString());
			}
		}

		#endregion Public Properties

		#region Public Constructors

		public TreePageItem()
		{
		}

		#endregion Public Constructors
	}
}