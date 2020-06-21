using System;

namespace FileExplorer.Models
{
	public interface IDispatcherService
	{
		#region Public Methods

		void Invoke(Action action);

		#endregion Public Methods
	}
}