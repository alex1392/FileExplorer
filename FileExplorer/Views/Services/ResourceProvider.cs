using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Views.Services
{
	public class ResourceProvider : IResourceProvider
	{
		public object FindResource(string resourceKey)
		{
			return App.Current.FindResource(resourceKey);
		}

		public object TryFindResource(string resourceKey)
		{
			return App.Current.TryFindResource(resourceKey);
		}
	}
}
