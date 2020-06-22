using FileExplorer.Models;
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
using System.Windows.Shapes;

namespace FileExplorer.Views
{
	
	/// <summary>
	/// Interaction logic for FileNameInputWindow.xaml
	/// </summary>
	public partial class FileNameInputWindow : Window
	{
		public class OkButtonCommand : ICommand
		{
			private readonly Action action;
			private readonly Func<bool> canAction;

			public event EventHandler CanExecuteChanged;
			public OkButtonCommand(Action action, Func<bool> canAction = null)
			{
				this.action = action;
				this.canAction = canAction;
			}
			public void OnCanExecuteChanged()
			{
				CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			}
			public bool CanExecute(object parameter)
			{
				return canAction == null || canAction.Invoke();
			}

			public void Execute(object parameter)
			{
				action.Invoke();
			}
		}
		public FileNameInputWindow()
		{
			InitializeComponent();
			this.DataContext = this;
			OkCommand = new OkButtonCommand(OK, CanOK);
		}

		public OkButtonCommand OkCommand { get; private set; }

		private void OK()
		{
			DialogResult = true;
			Close();
		}

		private bool CanOK()
		{
			return !string.IsNullOrEmpty(fileNameTextBox.Text);
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void fileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			OkCommand.OnCanExecuteChanged();
		}
	}
}
