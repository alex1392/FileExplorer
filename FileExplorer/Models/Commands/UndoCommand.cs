using System;

namespace FileExplorer.Models
{
	public class UndoCommand : IUndoCommand
	{
		#region Private Fields

		private readonly Func<bool> action;
		private readonly Func<bool> undoAction;
		private readonly Func<bool> canAction;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Properties

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public UndoCommand(Func<bool> action, Func<bool> undoAction, Func<bool> canAction = null)
		{
			this.action = action ?? throw new ArgumentNullException(nameof(action));
			this.undoAction = undoAction ?? throw new ArgumentNullException(nameof(undoAction));
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
			IsExecutionSuccessful = action.Invoke();
		}

		public void Undo()
		{
			IsUndoSuccessful = undoAction.Invoke();
		}

		#endregion Public Methods
	}

	public class UndoCommand<TParam> : IUndoCommand
	{
		#region Private Fields

		private readonly Func<TParam, bool> action;
		private readonly Func<bool> undoAction;
		private readonly Func<TParam, bool> canAction;

		#endregion Private Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Properties

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public UndoCommand(Func<TParam, bool> action, Func<bool> undoAction, Func<TParam, bool> canAction = null)
		{
			this.action = action ?? throw new ArgumentNullException(nameof(action));
			this.undoAction = undoAction ?? throw new ArgumentNullException(nameof(undoAction));
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
			IsExecutionSuccessful = action.Invoke((TParam)parameter);
		}

		public void Undo()
		{
			IsUndoSuccessful = undoAction.Invoke();
		}

		#endregion Public Methods
	}
}