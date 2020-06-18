using System.IO;

namespace FileExplorer.Models
{
	public interface IFileProvider
	{
		void Copy(string sourcePath, string destPath);
		void Delete(string path);
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
		void Move(string sourcePath, string destPath);

		#endregion Public Methods
	}
}