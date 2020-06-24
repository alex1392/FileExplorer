using System;

namespace FileExplorer.Models
{
	public class UndoCommand : IUndoCommand
	{
		private readonly Func<bool> action;
		private readonly Func<bool> undoAction;
		private readonly Func<bool> canAction;

		public UndoCommand(Func<bool> action, Func<bool> undoAction, Func<bool> canAction = null)
		{
			this.action = action ?? throw new ArgumentNullException(nameof(action));
			this.undoAction = undoAction ?? throw new ArgumentNullException(nameof(undoAction));
			this.canAction = canAction;
		}

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		public event EventHandler CanExecuteChanged;

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
	}

	public class UndoCommand<TParam> : IUndoCommand
	{
		private readonly Func<TParam, bool> action;
		private readonly Func<bool> undoAction;
		private readonly Func<TParam, bool> canAction;

		public UndoCommand(Func<TParam, bool> action, Func<bool> undoAction, Func<TParam, bool> canAction = null)
		{
			this.action = action ?? throw new ArgumentNullException(nameof(action));
			this.undoAction = undoAction ?? throw new ArgumentNullException(nameof(undoAction));
			this.canAction = canAction;
		}

		public bool IsExecutionSuccessful { get; private set; }

		public bool IsUndoSuccessful { get; private set; }

		public event EventHandler CanExecuteChanged;

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
	}
}