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

		public OkButtonCommand OkCommand { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public FileNameInputWindow()
		{
			InitializeComponent();
			this.DataContext = this;
			OkCommand = new OkButtonCommand(OK, CanOK);
		}

		#endregion Public Constructors

		#region Private Methods

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

		#endregion Private Methods

		#region Public Classes

		public class OkButtonCommand : ICommand
		{
			#region Private Fields

			private readonly Action action;
			private readonly Func<bool> canAction;

			#endregion Private Fields

			#region Public Events

			public event EventHandler CanExecuteChanged;

			#endregion Public Events

			#region Public Constructors

			public OkButtonCommand(Action action, Func<bool> canAction = null)
			{
				this.action = action;
				this.canAction = canAction;
			}

			#endregion Public Constructors

			#region Public Methods

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

			#endregion Public Methods
		}

		#endregion Public Classes
	}
}