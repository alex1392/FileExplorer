using System;
using System.Collections.Generic;

namespace FileExplorer.Models
{
	public abstract class PasteCommand : IUndoCommand
	{
		#region Protected Fields

		protected readonly IFileProvider fileProvider;
		protected readonly INavigationService navigationService;

		#endregion Protected Fields

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion Public Events

		#region Public Properties

		public bool IsExecutionSuccessful { get; protected set; }

		public bool IsUndoSuccessful { get; protected set; }
		public List<string> SourcePaths { get; set; }

		public string DestPath { get; set; }

		#endregion Public Properties

		#region Public Constructors

		public PasteCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.navigationService = navigationService;
			this.fileProvider = fileProvider;
		}

		#endregion Public Constructors

		#region Public Methods

		public abstract bool CanExecute(object parameter);

		public abstract void Execute(object parameter);

		public abstract void Undo();

		#endregion Public Methods
	}
}