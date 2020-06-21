
using System;
using System.Collections.Generic;

namespace FileExplorer.Models
{
	public abstract class PasteCommand : IUndoCommand
	{
		protected readonly IFileProvider fileProvider;
		protected readonly INavigationService navigationService;

		public bool IsExecutionSuccessful { get; protected set; }

		public bool IsUndoSuccessful { get; protected set; }

		public event EventHandler CanExecuteChanged;

		public List<string> SourcePaths { get; set; }

		public string DestPath { get; set; }

		public PasteCommand(IFileProvider fileProvider, INavigationService navigationService)
		{
			this.navigationService = navigationService;
			this.fileProvider = fileProvider;
		}

		public abstract bool CanExecute(object parameter);
		public abstract void Execute(object parameter);
		public abstract void Undo();
	}

	
}