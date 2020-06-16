namespace FileExplorer.Models
{
	public interface ITypeDescriptionProvider
	{
		#region Public Methods

		string GetFileTypeDescription(string fileNameOrExtension);

		string GetFolderTypeDescription(string folderPath);

		#endregion Public Methods
	}
}