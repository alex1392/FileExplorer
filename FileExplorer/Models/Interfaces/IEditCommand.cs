using System;
using System.Windows.Input;

namespace FileExplorer.Models
{
	public interface IUndoCommand : ICommand
	{
		bool IsExecutionSuccessful { get; }
		bool IsUndoSuccessful { get; }

		public void Undo();
	}
}