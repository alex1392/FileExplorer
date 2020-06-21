using System;
using System.IO;

namespace FileExplorer.Models
{
	[Serializable]
	public class PathNotFoundException : Exception
	{
		#region Public Constructors

		public PathNotFoundException() { }

		public PathNotFoundException(string message) : base(message) { }

		public PathNotFoundException(string message, Exception inner) : base(message, inner) { }

		#endregion Public Constructors

		#region Protected Constructors

		protected PathNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

		#endregion Protected Constructors
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

		/// <summary>
		/// Move file or folder.
		/// </summary>
		/// <param name="sourcePath">Path of file or folder to be moved.</param>
		/// <param name="destPath">Path of the target folder.</param>
		/// <returns>Return bool indicates of this operation is successful.</returns>
		public bool Move(string sourcePath, string destPath)
		{
			if (sourcePath == destPath)
			{
				return false;
			}
			try
			{
				var name = Path.GetFileName(sourcePath);
				Directory.Move(sourcePath, Path.Combine(destPath, name));
				return true;
			}
			catch (UnauthorizedAccessException ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			catch (IOException ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			return false;
		}

		/// <summary>
		/// Copy file or folder.
		/// </summary>
		/// <param name="sourcePath">Path of file or folder to be copyed.</param>
		/// <param name="destPath">Path of the destination of file or folder to be copyed.</param>
		/// <returns>The path that has been copyed, returns null if this operation was failed.</returns>
		public string Copy(string sourcePath, string destPath)
		{
			if (File.Exists(sourcePath))
			{
				return CopyFile(sourcePath, destPath);
			}
			else if (Directory.Exists(sourcePath))
			{
				return CopyFolder(sourcePath, destPath);
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
					path = Path.Combine(dir, $"{name} ({i}){ext}");
					i++;
				}
				return path;
			}

			string CopyFolder(string sourcePath, string destPath)
			{
				if (Directory.Exists(destPath))
				{
					destPath = RenamePath(destPath);
				}
				try
				{
					Directory.CreateDirectory(destPath);
				}
				catch (UnauthorizedAccessException ex)
				{
					dialogService.ShowMessage(ex.Message);
					return null;
				}
				catch (IOException ex)
				{
					dialogService.ShowMessage(ex.Message);
					return null;
				}

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
				return destPath;
			}

			string CopyFile(string sourcePath, string destPath)
			{
				if (File.Exists(destPath))
				{
					destPath = RenamePath(destPath);
				}
				try
				{
					File.Copy(sourcePath, destPath);
					return destPath;
				}
				catch (UnauthorizedAccessException ex)
				{
					dialogService.ShowMessage(ex.Message);
				}
				catch (IOException ex)
				{
					dialogService.ShowMessage(ex.Message);
				}
				return null;
			}
		}

		/// <summary>
		/// Delete file or folder (recursively).
		/// </summary>
		/// <param name="path">Path of file or folder to be deleted.</param>
		/// <returns>Bool indicates this operation is successful or not.</returns>
		public bool Delete(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				else if (Directory.Exists(path))
				{
					Directory.Delete(path, recursive: true);
				}
				return true;
			}
			catch (UnauthorizedAccessException ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			catch (IOException ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			return false;
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

		public string GetFileName(string path)
		{
			return Path.GetFileName(path);
		}

		#endregion Public Methods
	}
}