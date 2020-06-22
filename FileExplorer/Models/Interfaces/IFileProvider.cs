using System.IO;

namespace FileExplorer.Models
{
	public interface IFileProvider
	{
		#region Public Methods

		string Copy(string sourcePath, string destPath);

		bool Delete(string path);

		#endregion Public Methods

		#region Public Methods

		(string[], string[]) GetChildren(string path);

		string[] GetDirectories(string path);

		FileInfo GetFileInfo(string path);

		string GetFileNameWithoutExtension(string path);

		string[] GetFiles(string path);

		FileSystemInfo GetFileSystemInfo(string path);

		string GetParent(string path);

		bool IsDirectoryExists(string path);

		bool IsFileExists(string path);

		string Move(string sourcePath, string destPath);

		string GetFileName(string path);
		string Create(string path);
		bool DeleteToBin(string path);
		bool RestoreFromBin(string path);

		#endregion Public Methods
	}
}