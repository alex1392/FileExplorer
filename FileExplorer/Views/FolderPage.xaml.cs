using FileExplorer.Models;
using FileExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileExplorer.Views {
	/// <summary>
	/// Interaction logic for FolderPage.xaml
	/// </summary>
	public partial class FolderPage : Page {
		private readonly FolderPageViewModel vm;

		public FolderPage()
		{
			InitializeComponent();
			vm = new FolderPageViewModel();
			DataContext = vm;
		}

		public FolderPage(TreeFolderItem folderItem) : this()
		{
			CurrentFolder = folderItem;
		}

		public TreeFolderItem CurrentFolder { get; }

		private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
		{

		}

		private void ListViewItem_Selected(object sender, RoutedEventArgs e)
		{

		}
	}
}
