using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Models
{
	public class UndoRedoManager
	{
		#region Private Fields

		private readonly List<ParamCommand> commands = new List<ParamCommand>();
		private int index = -1;

		#endregion Private Fields

		#region Public Properties

		public ParamCommand CurrentCommand
		{
			get
			{
				if (commands.Count == 0 || index < 0 || index >= commands.Count)
				{
					return ParamCommand.Empty;
				}
				return commands[index];
			}
		}

		public int Capacity { get; }
		public RelayCommand UndoCommand { get; private set; }
		public RelayCommand RedoCommand { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public UndoRedoManager(int capacity = 10)
		{
			Capacity = capacity;

			UndoCommand = new RelayCommand(Undo, CanUndo);
			RedoCommand = new RelayCommand(Redo, CanRedo);
		}

		#endregion Public Constructors

		#region Public Methods

		public IEnumerable<ParamCommand> GetUndoEnumerble() => commands.Take(index + 1);

		public IEnumerable<ParamCommand> GetRedoEnumerable() => commands.Skip(index + 1);

		public void Execute(IUndoCommand newCommand, object parameter = null)
		{
			// execute new command
			newCommand.Execute(parameter);
			// if new command is not successfully executed, don't add it into list
			if (!newCommand.IsExecutionSuccessful)
			{
				return;
			}
			// remove redo commands
			commands.RemoveRange(index + 1, commands.Count - (index + 1));
			// add new command and its parameter to list
			commands.Add(new ParamCommand(newCommand, parameter));
			// move index forward
			index++;
			// if number of command is larger than the capacity of the list
			if (commands.Count > Capacity)
			{
				// remove the oldest command
				commands.RemoveAt(0);
			}
			// raise canRedoChanged event
			RedoCommand.RaiseCanExecuteChanged();
		}

		#endregion Public Methods

		#region Private Methods

		private bool CanUndo() => index >= 0;

		private bool CanRedo() => index < commands.Count - 1;

		private void Undo()
		{
			// execute undo
			CurrentCommand.Command.Undo();
			// if undo is not successful
			if (!CurrentCommand.Command.IsUndoSuccessful)
			{
				// remove the current command
				commands.Remove(CurrentCommand);
			}
			// move the index to the previous command
			index--;
			// if cannot undo...
			if (!CanUndo())
			{
				// raise canUndoChanged event
				UndoCommand.RaiseCanExecuteChanged();
			}
		}

		private void Redo()
		{
			// move the index to the next command
			index++;
			// execute redo with stored parameter
			CurrentCommand.Command.Execute(CurrentCommand.Parameter);
			// if redo is not successful
			if (!CurrentCommand.Command.IsExecutionSuccessful)
			{
				// remove the current command
				commands.Remove(CurrentCommand);
				// if removal of command causes index out of boundary
				if (index == commands.Count)
				{
					// move the index back
					index--;
				}
			}
			// if cannot redo...
			if (!CanRedo())
			{
				// raise canRedoChanged event
				RedoCommand.RaiseCanExecuteChanged();
			}
		}

		#endregion Private Methods
	}

	public struct ParamCommand
	{
		#region Public Fields

		public static readonly ParamCommand Empty = new ParamCommand();

		#endregion Public Fields

		#region Public Properties

		public IUndoCommand Command { get; private set; }

		public object Parameter { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		public ParamCommand(IUndoCommand command, object parameter)
		{
			Command = command ?? throw new ArgumentNullException(nameof(command));
			Parameter = parameter;
		}

		#endregion Public Constructors
	}
}