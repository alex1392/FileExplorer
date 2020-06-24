using System;

namespace FileExplorer.Models
{
	public interface IDispatcherService
	{
		#region Public Methods

		void Invoke(Action action);
		TResult Invoke<TResult>(Func<TResult> func);

		#endregion Public Methods
	}
}