using Microsoft.VisualBasic.FileIO;
using Shell32;
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
		/// Create file or folder.
		/// </summary>
		/// <param name="path">The path of file or folder to be created.</param>
		/// <returns>The path of created file or folder, returns null if this operation failed. </returns>
		public string Create(string path)
		{
			try
			{
				if (IsDirectory(path))
				{
					if (Directory.Exists(path))
					{
						path = RenamePath(path);
					}
					Directory.CreateDirectory(path);
				}
				else
				{
					if (File.Exists(path))
					{
						path = RenamePath(path);
					}
					File.Create(path);
				}
				return path;
			}
			catch (Exception ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			return null;
		}


		/// <summary>
		/// Move file or folder.
		/// </summary>
		/// <param name="sourcePath">Path of file or folder to be moved.</param>
		/// <param name="destPath">Destination path.</param>
		/// <returns>Return bool indicates of this operation is successful.</returns>
		public string Move(string sourcePath, string destPath)
		{
			if (!File.Exists(sourcePath) && 
				!Directory.Exists(sourcePath))
			{
				throw new PathNotFoundException();
			}
			try
			{
				Directory.Move(sourcePath, destPath);
				return destPath;
			}
			catch (Exception ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			return null;
			
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
				catch (Exception ex)
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
				catch (Exception ex)
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
			catch (Exception ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			return false;
		}

		#region Delete/Restore with recycle bin

		/// <summary>
		/// Move file or folder to recycle bin.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool DeleteToBin(string path)
		{
			try
			{
				if (IsDirectory(path))
				{
					FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
				}
				else
				{
					FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
				}
				return true;
			}
			catch (Exception ex)
			{
				dialogService.ShowMessage(ex.Message);
			}
			return false;
		}

		private Shell Shl;
		private const long ssfBITBUCKET = 10;
		private const int recycleNAME = 0;
		private const int recyclePATH = 1;
		public bool RestoreFromBin(string path)
		{
			Shl = new Shell();
			var Recycler = Shl.NameSpace(10);
			var recycledItems = Recycler.Items();
			for (var i = 0; i < recycledItems.Count; i++)
			{
				var FI = recycledItems.Item(i);
				var FileName = Recycler.GetDetailsOf(FI, 0);
				if (Path.GetExtension(FileName) == "") 
					FileName += Path.GetExtension(FI.Path);
				//Necessary for systems with hidden file extensions.
				var FilePath = Recycler.GetDetailsOf(FI, 1);
				if (path == Path.Combine(FilePath, FileName))
				{
					// TODO: localisation??
					if (DoVerb(FI, "ESTORE") || DoVerb(FI, "還原(&E)"))
						return true;
				}
			}
			return false;

			static bool DoVerb(FolderItem Item, string Verb)
			{
				foreach (FolderItemVerb FIVerb in Item.Verbs())
				{
					if (FIVerb.Name.ToUpper().Contains(Verb.ToUpper()))
					{
						FIVerb.DoIt();
						return true;
					}
				}
				return false;
			}
		}
		#endregion
		private static bool IsDirectory(string path)
		{
			return string.IsNullOrEmpty(Path.GetExtension(path));
		}
		private string RenamePath(string path)
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


		public bool IsDirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public bool IsFileExists(string path)
		{
			return File.Exists(path);
		}

		#endregion Public Methods
	}
}