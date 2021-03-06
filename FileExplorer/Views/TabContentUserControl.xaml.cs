﻿using FileExplorer.Models;
using FileExplorer.ViewModels;
using FileExplorer.Views.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using IO = System.IO;

namespace FileExplorer.Views
{
	/// <summary>
	/// Interaction logic for TabContentUserControl.xaml
	/// </summary>
	public partial class TabContentUserControl : UserControl, INotifyPropertyChanged
	{
		#region Private Fields

		private readonly TabContentViewModel vm;
		private readonly IDialogService dialogService;
		private readonly INavigationService navigationService;
		private readonly UndoRedoManager undoRedoManager;
		private readonly EditManager editManager;
		private PasteType pasteType;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public object CurrentFolder => (FolderFrame.Content as Page)?.Title;

		#endregion Public Properties

		#region Private Properties

		private ListView CurrentView => (FolderFrame.Content as FolderPage)?.ItemsListView;

		#endregion Private Properties

		#region Public Constructors

		public TabContentUserControl()
		{
			InitializeComponent();
			Loaded += TabContentUserControl_Loaded;
		}

		public TabContentUserControl(TabContentViewModel vm, IDialogService dialogService, INavigationService navigationService, UndoRedoManager undoRedoManager, EditManager editManager) : this()
		{
			this.vm = vm;
			this.dialogService = dialogService;
			this.navigationService = navigationService;
			this.undoRedoManager = undoRedoManager;
			this.editManager = editManager;
			DataContext = vm;
			navigationService.NavigatedPageLoaded += NavigationService_NavigatedPageLoaded;
			if (navigationService is FolderNavigationService folderNavigationService)
			{
				folderNavigationService.AddFrame(FolderFrame);
			}
		}

		#endregion Public Constructors

		#region Private Methods
		private void TabContentUserControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (navigationService.Content == null)
			{
				navigationService.Navigate(vm.HomePage.Uri);
			}
		}
		private void NavigationService_NavigatedPageLoaded(object sender, EventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFolder)));
		}

		private void ViewButton_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is MenuItem menuItem) ||
				!(FolderFrame.Content is FolderPage folderPage))
			{
				return;
			}
			folderPage.ChangeView(menuItem.Header.ToString());
		}

		private void PathListBox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				PathTextBox.Visibility = Visibility.Visible;
				Keyboard.Focus(PathTextBox);
			}
		}

		private void PathTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			PathTextBox.Visibility = Visibility.Collapsed;
		}

		private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is TreeFolderItem folderItem))
			{
				return;
			}
			folderItem.LoadSubFolders();
		}

		private void NavigationComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is ComboBox comboBox))
			{
				return;
			}
			var binding = comboBox.GetBindingExpression(ComboBox.SelectedItemProperty);
			binding.UpdateTarget();
		}

		private void PreviewToggleButton_Click(object sender, RoutedEventArgs e)
		{
			PreviewGridColumn.Width = GridLength.Auto;
		}

		private void TreeViewToggleButton_Click(object sender, RoutedEventArgs e)
		{
			TreeViewGridColumn.Width = GridLength.Auto;
		}

		private void Copy(object sender, ExecutedRoutedEventArgs e)
		{
			SetClipBoard();
			pasteType = PasteType.Copy;
		}

		private void CanCopy(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CurrentView.SelectedItems.Count > 0;
		}

		private void Cut(object sender, ExecutedRoutedEventArgs e)
		{
			SetClipBoard();
			pasteType = PasteType.Cut;
		}

		private void CanCut(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CurrentView.SelectedItems.Count > 0;
		}

		private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Clipboard.GetFileDropList().Count > 0;
		}

		private void CanNew(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void SetClipBoard()
		{
			Clipboard.Clear();
			var paths = new StringCollection();
			paths.AddRange(GetSelectedPaths().ToArray());
			Clipboard.SetFileDropList(paths);
		}

		private IEnumerable<string> GetSelectedPaths()
		{
			return CurrentView.SelectedItems.OfType<ListItemViewModel>().Select(vm => vm.Path);
		}

		private void PathItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as ListBoxItem)?.DataContext is Item item))
			{
				return;
			}
			if (item.Path == vm.CurrentPath)
			{
				return;
			}
			navigationService.Navigate(nameof(FolderPage), item.Path);
		}

		private void PathTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				navigationService.Navigate(nameof(FolderPage), PathTextBox.Text);
			}
		}

		private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is ITreeItem treeItem))
			{
				return;
			}
			if (treeItem is TreeFolderItem folderItem)
			{
				// load the subfolders for the tree view item to expand
				folderItem.LoadSubFolders();
				// Navigate to the selected folder
				navigationService.Navigate(nameof(FolderPage), folderItem.Path);
			}
			else if (treeItem is TreePageItem pageItem)
			{
				navigationService.Navigate(pageItem.Uri);
			}
			// avoid recursive event propagation
			if (e != null)
			{
				e.Handled = true;
			}
		}

		private void Undo(object sender, ExecutedRoutedEventArgs e)
			=> undoRedoManager.UndoCommand.Execute(e.Parameter);

		private void Redo(object sender, ExecutedRoutedEventArgs e)
	=> undoRedoManager.RedoCommand.Execute(e.Parameter);

		private void CanRedo(object sender, CanExecuteRoutedEventArgs e)
	=> e.CanExecute = undoRedoManager.RedoCommand.CanExecute(e.Parameter);

		private void CanUndo(object sender, CanExecuteRoutedEventArgs e)
	=> e.CanExecute = undoRedoManager.UndoCommand.CanExecute(e.Parameter);

		private void Paste(object sender, ExecutedRoutedEventArgs e)
		{
			var sourcePaths = Clipboard.GetFileDropList().OfType<string>().ToList();
			var isSuccessful = editManager.Paste(sourcePaths, vm.CurrentPath, pasteType);
			if (isSuccessful)
			{
				Clipboard.Clear();
			}
		}

		private void New(object sender, ExecutedRoutedEventArgs e)
		{
			var ext = e.Parameter?.ToString();
			var (result, filename) = dialogService.ShowFileNameDialog();
			if (!result)
			{
				return;
			}
			if (string.IsNullOrEmpty(filename))
			{
				// set default filename
				filename = ext == null ? "New Folder" : "New File";
			}
			// remove extension
			var extensionIndex = filename.IndexOf('.');
			if (extensionIndex > 0)
			{
				filename = filename.Remove(extensionIndex);
			}
			editManager.New(IO::Path.Combine(vm.CurrentPath, filename + ext));
		}

		private void Delete(object sender, ExecutedRoutedEventArgs e)
		{
			editManager.Delete(GetSelectedPaths().ToList());
		}

		private void BrowseBack(object sender, ExecutedRoutedEventArgs e)
		{
			navigationService.GoBack();
		}

		private void CanBrowseBack(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = navigationService.CanGoBack;
		}

		private void BrowseForward(object sender, ExecutedRoutedEventArgs e)
		{
			navigationService.GoForward();
		}

		private void CanBrowseForward(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = navigationService.CanGoForward;
		}

		#endregion Private Methods
	}
}