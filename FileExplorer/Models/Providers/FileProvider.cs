using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;

namespace FileExplorer.Models
{
	[Serializable]
	public class PathNotFoundException : Exception
	{
		public PathNotFoundException() { }
		public PathNotFoundException(string message) : base(message) { }
		public PathNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected PathNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}

	public class FileProvider : IFileProvider
	{
		#region Private Fields

		private readonly IDialogService dialogService;

		#endregion Private Fields

		#region Public Constructors

		public FileProvider(IDialogService dialogService)
		{
			this.dialogService = dialogService;
		}

		#endregion Public Constructors

		#region Public Methods

		public void Move(string sourcePath, string destPath)
		{
			if (sourcePath == destPath)
			{
				return;
			}
			try
			{
				Directory.Move(sourcePath, destPath);
			}
			catch (UnauthorizedAccessException ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			catch (IOException ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
		}

		public void Copy(string sourcePath, string destPath)
		{
			if (File.Exists(sourcePath))
			{
				CopyFile(sourcePath, destPath);
			}
			else if (Directory.Exists(sourcePath))
			{
				CopyFolder(sourcePath, destPath);
			}
			else
			{
				throw new PathNotFoundException();
			}

			static string RenamePath(string path)
			{
				var i = 2;
				var ext = Path.GetExtension(path);
				var dir = Path.GetDirectoryName(path);
				var name = Path.GetFileNameWithoutExtension(path);
				while (File.Exists(path) || Directory.Exists(path))
				{
					path = Path.Combine(dir, $"{name} ({i})", ext);
					i++;
				}
				return path;
			}

			static void CopyFolder(string sourcePath, string destPath)
			{
				if (Directory.Exists(destPath))
				{
					destPath = RenamePath(destPath);
				}
				Directory.CreateDirectory(destPath);

				var folders = Directory.GetDirectories(sourcePath);
				foreach (var folder in folders)
				{
					var name = Path.GetFileName(folder);
					CopyFolder(folder, Path.Combine(destPath, name));
				}

				var files = Directory.GetFiles(sourcePath);
				foreach (var file in files)
				{
					var name = Path.GetFileName(file);
					CopyFile(file, Path.Combine(destPath, name));
				}
			}

			static void CopyFile(string sourcePath, string destPath)
			{
				if (File.Exists(destPath))
				{
					destPath = RenamePath(destPath);
				}
				File.Copy(sourcePath, destPath);
			}
		}

		public void Delete(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			else if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
			else
			{
				throw new PathNotFoundException();
			}
		}

		public (string[], string[]) GetChildren(string path)
		{
			try
			{
				return (Directory.GetDirectories(path), Directory.GetFiles(path));
			}
			catch (UnauthorizedAccessException ex)
			{
				dialogService.ShowMessage(ex.Message);
				return (new string[0], new string[0]);
			}
		}

		public string[] GetDirectories(string path)
		{
			try
			{
				return Directory.GetDirectories(path);
			}
			catch (UnauthorizedAccessException ex)
			{
				dialogService.ShowMessage(ex.Message);
				return new string[0];
			}
		}

		public FileInfo GetFileInfo(string path)
		{
			return new FileInfo(path);
		}

		public string[] GetFiles(string path)
		{
			try
			{
				return Directory.GetFiles(path);
			}
			catch (UnauthorizedAccessException ex)
			{
				dialogService.ShowMessage(ex.Message);
				return new string[0];
			}
		}

		public FileSystemInfo GetFileSystemInfo(string path)
		{
			if (Directory.Exists(path))
			{
				return new DirectoryInfo(path);
			}
			else if (File.Exists(path))
			{
				return new FileInfo(path);
			}
			else
			{
				throw new PathNotFoundException();
			}
		}

		public string GetParent(string path)
		{
			return Path.GetDirectoryName(path);
		}

		public bool IsDirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public bool IsFileExists(string path)
		{
			return File.Exists(path);
		}

		public string GetFileNameWithoutExtension(string path)
		{
			return Path.GetFileNameWithoutExtension(path);
		}

		#endregion Public Methods
	}
}