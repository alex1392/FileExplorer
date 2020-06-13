using System.IO;

namespace FileExplorer.Models {

	public interface IFileProvider {

		#region Public Methods

		(string[], string[]) GetChildren(string path);

		string[] GetDirectories(string path);

		FileInfo GetFileInfo(string path);

		string[] GetFiles(string path);

		FileSystemInfo GetFileSystemInfo(string path);
		string GetParent(string path);
		bool IsDirectoryExists(string path);

		bool IsFileExists(string path);

		#endregion Public Methods
	}
}