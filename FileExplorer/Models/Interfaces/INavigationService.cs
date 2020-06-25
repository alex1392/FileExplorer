using System;
using System.Collections;

namespace FileExplorer.Models
{
	public interface INavigationService
	{
		#region Public Events

		/// <summary>
		/// Raised when navigation process completed, pass the destination path as event argument.
		/// </summary>
		event EventHandler<string> Navigated;

		event EventHandler NavigatedPageLoaded;

		event EventHandler GoBackCompleted;

		event EventHandler GoForwardCompleted;

		#endregion Public Events

		#region Public Properties

		IEnumerable BackStack { get; }
		bool CanGoBack { get; }

		bool CanGoForward { get; }

		bool CanGoUp { get; }

		/// <summary>
		/// Get or set the current content.
		/// </summary>
		object Content { get; set; }

		IEnumerable ForwardStack { get; }

		#endregion Public Properties

		#region Public Methods

		void GoBack();

		void GoForward();

		void GoUp();

		/// <summary>
		/// Process navigation
		/// </summary>
		/// <param name="pageKey">A string indicates which page will be navigated to.</param>
		/// <param name="path">The folder path to be navigated.</param>
		void Navigate(string pageKey, string path);

		void Navigate(Uri uri);

		void Refresh();

		#endregion Public Methods
	}
}