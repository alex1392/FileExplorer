using System.IO;

namespace FileExplorer.Models {
	public interface IFileProvider {
		string[] GetFiles(string path);
		string[] GetDirectories(string path);
		FileSystemInfo GetFileSystemInfo(string path);
		FileInfo GetFileInfo(string path);
		(string[], string[]) GetChildren(string path);
		bool IsFileExists(string path);
		bool IsDirectoryExists(string path);
	}
}
