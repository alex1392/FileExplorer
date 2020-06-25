using FileExplorer.Models;

using System;
using System.IO;

namespace FileExplorer.Views.Services
{
	public class FileWatcherService : IFileWatcherService, IDisposable
	{
		#region Private Fields

		private readonly FileSystemWatcher watcher = new FileSystemWatcher();
		private readonly INavigationService navigationService;
		private readonly IDispatcherService dispatcherService;

		#endregion Private Fields

		#region Public Properties

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

		#endregion Public Properties

		#region Public Constructors

		public FileWatcherService(INavigationService navigationService, IDispatcherService dispatcherService)
		{
			watcher.Changed += Refresh;
			watcher.Created += Refresh;
			watcher.Deleted += Refresh;
			watcher.Renamed += Refresh;
			this.navigationService = navigationService;
			this.dispatcherService = dispatcherService;
		}

		#endregion Public Constructors

		#region Public Methods

		public void Start()
		{
			watcher.EnableRaisingEvents = true;
		}

		public void Dispose()
		{
			watcher.Dispose();
		}

		#endregion Public Methods

		#region Private Methods

		private void Refresh(object sender, EventArgs e)
		{
			dispatcherService.Invoke(() => navigationService.Refresh());
		}

		#endregion Private Methods
	}
}