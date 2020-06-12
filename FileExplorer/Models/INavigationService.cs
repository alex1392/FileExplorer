namespace FileExplorer.Models {
	public interface IFolderNavigationService {
		void Navigate(string key);
		void GoBack();
		void GoForward();
	}
}
