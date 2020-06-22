using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Views.Services
{
	public class FileWatcherService : IFileWatcherService, IDisposable
	{
		private readonly FileSystemWatcher watcher = new FileSystemWatcher();
		private readonly INavigationService navigationService;
		private readonly IDispatcherService dispatcherService;

		public string Path
		{
			get => watcher.Path; 
			set
			{
				if (value == watcher.Path)
				{
					return;
				}
				watcher.Path = value;
			}
		}

		public FileWatcherService(INavigationService navigationService, IDispatcherService dispatcherService)
		{
			watcher.Changed += Refresh;
			watcher.Created += Refresh;
			watcher.Deleted += Refresh;
			watcher.Renamed += Refresh;
			this.navigationService = navigationService;
			this.dispatcherService = dispatcherService;
		}

		public void Start()
		{
			watcher.EnableRaisingEvents = true;
		}

		private void Refresh(object sender, EventArgs e)
		{
			dispatcherService.Invoke(() => navigationService.Refresh());
		}

		public void Dispose()
		{
			watcher.Dispose();
		}
	}
}
