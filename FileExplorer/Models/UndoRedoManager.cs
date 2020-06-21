using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public class UndoRedoManager
	{
		#region Private Fields

		private readonly List<IUndoCommand> commands = new List<IUndoCommand>();
		private int index = -1;

		#endregion Private Fields

		#region Public Properties

		public IUndoCommand CurrentCommand
		{
			get
			{
				if (commands.Count == 0 || index < 0 || index >= commands.Count)
				{
					return null;
				}
				return commands[index];
			}
		}

		public int Capacity { get; }
		public ICommand UndoCommand { get; private set; }
		public ICommand RedoCommand { get; private set; }

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

		public IEnumerable<IUndoCommand> GetUndoEnumerble() => commands.Take(index + 1);

		public IEnumerable<IUndoCommand> GetRedoEnumerable() => commands.Skip(index + 1);

		public void Execute(IUndoCommand newCommand)
		{
			newCommand.Execute(null);
			// if new command is not successfully executed, don't add it into list
			if (!newCommand.IsExecutionSuccessful)
			{
				return;
			}
			// remove redo commands
			commands.RemoveRange(index + 1, commands.Count - (index + 1));
			// add new command to list
			commands.Add(newCommand);
			// move index forward
			index++;
			// if number of command is larger than the capacity of the list
			if (commands.Count > Capacity)
			{
				// remove the oldest command
				commands.RemoveAt(0);
			}
		}

		#endregion Public Methods

		#region Private Methods

		private bool CanUndo() => index >= 0;

		private bool CanRedo() => index < commands.Count - 1;

		private void Undo()
		{
			if (!CanUndo())
			{
				throw new InvalidOperationException();
			}
			CurrentCommand.Undo();
			// if undo is not successful
			if (!CurrentCommand.IsUndoSuccessful)
			{
				// remove the current command
				commands.Remove(CurrentCommand);
			}
			// move the index to the previous command
			index--;
		}

		private void Redo()
		{
			if (!CanRedo())
			{
				throw new InvalidOperationException();
			}
			index++;
			CurrentCommand.Execute(null);
			// if redo is not successful
			if (!CurrentCommand.IsExecutionSuccessful)
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
		}

		#endregion Private Methods
	}
}