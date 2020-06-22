using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	internal class RelayCommand<TParam> : ICommand
	{
		private readonly Action<TParam> action;
		private readonly Func<TParam, bool> canAction;
		/// <summary>
		/// This event will never be called.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		public RelayCommand(Action<TParam> action, Func<TParam, bool> canAction = null)
		{
			this.action = action;
			this.canAction = canAction;
		}

		public bool CanExecute(object parameter)
		{
			return canAction == null || canAction.Invoke((TParam)parameter);
		}

		public void Execute(object parameter)
		{
			action?.Invoke((TParam)parameter);
		}
	}
	internal class RelayCommand : ICommand
	{
		#region Private Fields

		private readonly Action action;
		private readonly Func<bool> canAction;

		#endregion Private Fields

		#region Public Events
		/// <summary>
		/// This event will never be called.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Constructors

		public RelayCommand(Action action, Func<bool> canAction = null)
		{
			this.action = action;
			this.canAction = canAction;
		}

		#endregion Public Constructors

		#region Public Methods

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
}