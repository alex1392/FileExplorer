namespace FileExplorer.Models {
	public interface IFolderNavigationService {
		void GoBack();
		void GoForward();
		void Navigate(string pageKey, object parameter);
	}
}
