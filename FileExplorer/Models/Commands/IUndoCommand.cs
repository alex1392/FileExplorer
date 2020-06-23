using System.Windows.Input;

namespace FileExplorer.Models
{
	public interface IUndoCommand : ICommand
	{
		#region Public Properties

		bool IsExecutionSuccessful { get; }
		bool IsUndoSuccessful { get; }

		#endregion Public Properties

		#region Public Methods

		public void Undo();

		#endregion Public Methods
	}
}