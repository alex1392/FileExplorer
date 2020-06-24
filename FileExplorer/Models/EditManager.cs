using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FileExplorer.Models
{
	public enum PasteType
	{
		Cut,
		Copy,
	}
	public class EditManager
	{
		private readonly IServiceProvider serviceProvider;
		private readonly IFileProvider fileProvider;
		private readonly INavigationService navigationService;
		private readonly UndoRedoManager undoRedoManager;

		public EditManager(IServiceProvider serviceProvider, IFileProvider fileProvider, INavigationService navigationService, UndoRedoManager undoRedoManager)
		{
			this.serviceProvider = serviceProvider;
			this.fileProvider = fileProvider;
			this.navigationService = navigationService;
			this.undoRedoManager = undoRedoManager;
		}
		public bool Paste(List<string> sourcePaths, string destPath, PasteType type)
		{
			PasteCommand command = type switch
			{
				PasteType.Cut => serviceProvider.GetService<CutPasteCommand>(),
				PasteType.Copy => serviceProvider.GetService<CopyPasteCommand>(),
				_ => throw new NotImplementedException(),
			};
			command.SourcePaths = sourcePaths;
			command.DestPath = destPath;
			undoRedoManager.Execute(command);
			return command.IsExecutionSuccessful;
		}

		public void New(string path)
		{
			string createdPath = null;
			var createCommand = new UndoCommand(New, UndoNew, CanNew);
			undoRedoManager.Execute(createCommand);

			bool New()
			{
				createdPath = fileProvider.Create(path);
				var result = createdPath != null;
				if (result)
				{
					navigationService.Refresh();
				}
				return result;
			}
			bool UndoNew()
			{
				var result = fileProvider.Delete(createdPath);
				if (result)
				{
					navigationService.Refresh();
				}
				return result;
			}
			bool CanNew()
			{
				return path != null;
			}
		}

		public void Delete(List<string> Paths)
		{
			var deleteCommand = new UndoCommand(Delete, UndoDelete, CanDelete);
			undoRedoManager.Execute(deleteCommand);

			bool Delete()
			{
				// delete every path in Paths to bin, if is not successful, remove the path from Paths.
				Paths.RemoveAll(path => !fileProvider.DeleteToBin(path));
				// if there's no any path successfully moved, the execution is not successful
				var result = Paths.Count > 0;
				// refresh page if successful
				if (result)
				{
					navigationService.Refresh();
				}
				return result;
			}

			bool UndoDelete()
			{
				// restore every path in Paths, if is not successful, remove the path from Paths
				Paths.RemoveAll(path => !fileProvider.RestoreFromBin(path));
				// if there's no any path successfully moved, the execution is not successful
				var result = Paths.Count > 0;
				// refresh page if successful
				if (result)
				{
					navigationService.Refresh();
				}
				return result;
			}

			bool CanDelete()
			{
				return Paths != null;
			}
		}

		public void Move(IEnumerable<string> sourcePath, string destPath)
		{
			var command = serviceProvider.GetService<CutPasteCommand>();
			command.SourcePaths = sourcePath.ToList();
			command.DestPath = destPath;
			undoRedoManager.Execute(command);
		}
	}
}
