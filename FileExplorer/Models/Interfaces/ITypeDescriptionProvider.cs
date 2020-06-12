namespace FileExplorer.Models {
	public interface ITypeDescriptionProvider {
		string GetFileTypeDescription(string fileNameOrExtension);
		string GetFolderTypeDescription(string folderPath);
	}
}