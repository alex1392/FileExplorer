namespace FileExplorer.Models {

	public interface IFolderNavigationService {

		#region Public Methods

		void GoBack();

		void GoForward();

		void Navigate(string pageKey, object parameter);

		#endregion Public Methods
	}
}