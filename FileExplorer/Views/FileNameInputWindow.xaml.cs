using FileExplorer.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileExplorer.Views
{
	/// <summary>
	/// Interaction logic for FileNameInputWindow.xaml
	/// </summary>
	public partial class FileNameInputWindow : Window
	{
		#region Public Properties

		public RelayCommand OkCommand { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public FileNameInputWindow()
		{
			InitializeComponent();
			this.DataContext = this;
			OkCommand = new RelayCommand(OK);
		}

		#endregion Public Constructors

		#region Private Methods

		private void OK()
		{
			DialogResult = true;
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void fileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			OkCommand.RaiseCanExecuteChanged();
		}

		#endregion Private Methods


	}
}