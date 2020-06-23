using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class RelayCommand<TParam> : ICommand
	{
		#region Private Fields

		private readonly Action<TParam> action;
		private readonly Func<TParam, bool> canAction;

		#endregion Private Fields

		#region Public Events

		/// <summary>
		/// This event will never be called.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Constructors

		public RelayCommand(Action<TParam> action, Func<TParam, bool> canAction = null)
		{
			this.action = action;
			this.canAction = canAction;
		}

		#endregion Public Constructors

		#region Public Methods

		public bool CanExecute(object parameter)
		{
			return canAction?.Invoke((TParam)parameter) ?? true;
		}

		public void Execute(object parameter)
		{
			action?.Invoke((TParam)parameter);
		}

		/// <summary>
		/// Invoke <see cref="CanExecuteChanged"/> to notify controls in the view.
		/// </summary>
		public void OnCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		#endregion Public Methods
	}

	public class RelayCommand : ICommand
	{
		#region Private Fields

		private readonly Action action;
		private readonly Func<bool> canAction;

		#endregion Private Fields

		#region Public Events

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
			return canAction?.Invoke() ?? true;
		}

		public void Execute(object parameter)
		{
			action?.Invoke();
		}
		/// <summary>
		/// Invoke <see cref="CanExecuteChanged"/> to notify controls in the view.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		#endregion Public Methods
	}
}