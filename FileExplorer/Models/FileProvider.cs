﻿using System;
using System.IO;

namespace FileExplorer.Models {
	class FileProvider : IFileProvider {
		private readonly IDialogService dialogService;

		public FileProvider(IDialogService dialogService)
		{
			this.dialogService = dialogService;
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
				dialogService.ShowMessage(ex.Message);
				return new string[0];
			}
		}
		public string[] GetFiles(string path)
		{
			try {
				return Directory.GetFiles(path);

			} catch (UnauthorizedAccessException ex) {
				dialogService.ShowMessage(ex.Message);
				return new string[0];
			}
		}
		public (string[], string[]) GetChildren(string path)
		{
			try {
				return (Directory.GetDirectories(path), Directory.GetFiles(path));
			} catch (UnauthorizedAccessException ex) {
				dialogService.ShowMessage(ex.Message);
				return (new string[0], new string[0]);
			}
		}
	}
}
