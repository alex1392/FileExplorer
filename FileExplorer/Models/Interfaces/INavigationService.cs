using FileExplorer.ViewModels;
using System;
using System.Windows.Navigation;

namespace FileExplorer.Models {

	public interface INavigationService {
		/// <summary>
		/// Get or set the current content.
		/// </summary>
		object Content { get; set; }
		bool CanGoBack { get; }
		bool CanGoForward { get; }
		bool CanGoUp { get; }

		/// <summary>
		/// Raised when navigation process completed, pass the destination path as event argument.
		/// </summary>
		event EventHandler<string> Navigated;

		#region Public Methods

		void GoBack();

		void GoForward();

		/// <summary>
		/// Process navigation
		/// </summary>
		/// <param name="pageKey">A string indicates which page will be navigated to.</param>
		/// <param name="path">The folder path to be navigated.</param>
		void Navigate(string pageKey, string path);
		void Refresh();
		void GoUp();

		#endregion Public Methods
	}
}