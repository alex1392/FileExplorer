using System;
using System.IO;

namespace FileExplorer.Models {
	class FileProvider : IFileProvider {
		private readonly IDialogService dialogService;

		public FileProvider(IDialogService dialogService)
		{
			this.dialogService = dialogService;
		}
		public bool IsDirectoryExists(string path)
		{
			return Directory.Exists(path);
		}
		public bool IsFileExists(string path)
		{
			return File.Exists(path);
		}
		public FileSystemInfo GetFileSystemInfo(string path)
		{
			FileSystemInfo info;
			if (Directory.Exists(path)) {
				info = new DirectoryInfo(path);
			} else if (File.Exists(path)) {
				info = new FileInfo(path);
			} else {
				throw new InvalidOperationException("Given path is not exist.");
			}
			return info;
		}
		public FileInfo GetFileInfo(string path)
		{
			return new FileInfo(path);
		}
		public string[] GetDirectories(string path)
		{
			try {
				return Directory.GetDirectories(path);
			} catch (UnauthorizedAccessException ex) {
				//dialogService.ShowMessage(ex.Message);
				return null;
			}
		}
		public string[] GetFiles(string path)
		{
			try {
				return Directory.GetFiles(path);

			} catch (UnauthorizedAccessException ex) {
				//dialogService.ShowMessage(ex.Message);
				return null;
			}
		}
		public (string[], string[]) GetChildren(string path)
		{
			try {
				return (Directory.GetDirectories(path), Directory.GetFiles(path));
			} catch (UnauthorizedAccessException ex) {
				// TODO: why the fuck a messageBox can cause error???
				//dialogService.ShowMessage(ex.Message);
				return (null, null);
			}
		}
	}
}
