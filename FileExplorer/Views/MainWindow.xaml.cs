using Cyc.FluentDesign;

using FileExplorer.Models;
using FileExplorer.ViewModels;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FileExplorer.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RevealWindow
	{
		#region Private Fields

		private readonly MainWindowViewModel vm;
		private readonly IDialogService dialogService;

		#endregion Private Fields

		private PasteType pasteType;
		private ListView CurrentView => (vm.CurrentContent as FolderPage).ItemsListView;

		#region Public Constructors

		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
		}

		public MainWindow(MainWindowViewModel vm, IDialogService dialogService) : this()
		{
			this.vm = vm;
			this.dialogService = dialogService;
			DataContext = this.vm;
		}

		#endregion Public Constructors

		#region Private Methods

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			vm.Navigate(vm.HomePage);
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

		private void HistoryItem_Selected(object sender, RoutedEventArgs e)
		{
			if (!(sender is ComboBoxItem comboBoxItem) ||
				!(comboBoxItem.DataContext is JournalEntry entry))
			{
				return;
			}
			var delta = NavigationComboBox.Items.IndexOf(entry) - NavigationComboBox.Items.IndexOf(vm.CurrentContent);
			while (delta > 0)
			{
				vm.GoForwardCommand.Execute(null);
				delta--;
			}
			while (delta < 0)
			{
				vm.GoBackCommand.Execute(null);
				delta++;
			}
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
			vm.Navigate(item);
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

		private void PathTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				vm.Navigate(PathTextBox.Text);
			}
		}

		private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
		{
			if (!((sender as TreeViewItem)?.DataContext is TreeFolderItem folderItem))
			{
				return;
			}
			folderItem.LoadSubFolders();
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
				vm.Navigate(folderItem);
			}
			else if (treeItem is TreePageItem pageItem)
			{
				vm.Navigate(pageItem);
			}
			// avoid recursive event propagation
			if (e != null)
			{
				e.Handled = true;
			}
		}

		#endregion Private Methods

		private void Undo(object sender, ExecutedRoutedEventArgs e)
		{
			vm.Undo(e.Parameter);
		}

		private void Redo(object sender, ExecutedRoutedEventArgs e)
		{
			vm.Redo(e.Parameter);
		}

		private void CanRedo(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = vm.CanRedo(e.Parameter);
		}

		private void CanUndo(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = vm.CanUndo(e.Parameter);
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

		private void Paste(object sender, ExecutedRoutedEventArgs e)
		{
			var sourcePaths = Clipboard.GetFileDropList().OfType<string>().ToList();
			var isSuccessful = vm.Paste(sourcePaths, vm.CurrentPath, pasteType);
			if (isSuccessful)
			{
				Clipboard.Clear();
			}
		}

		private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Clipboard.GetFileDropList().Count > 0;
		}

		private void New(object sender, ExecutedRoutedEventArgs e)
		{
			var ext = e.Parameter?.ToString();
			var filename = dialogService.ShowFileNameDialog();
			if (filename == null)
			{
				return;
			}
			// remove extension
			var extensionIndex = filename.IndexOf('.');
			if (extensionIndex > 0)
			{
				filename = filename.Remove(extensionIndex);
			}
			vm.New(Path.Combine(vm.CurrentPath, filename + ext));
		}

		private void CanNew(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void Delete(object sender, ExecutedRoutedEventArgs e)
		{
			vm.Delete(GetSelectedPaths().ToList());
		}

		private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
	}
}