using FileExplorer.Models;

using System;
using System.Windows;
using System.Windows.Threading;

namespace FileExplorer.Views.Services
{
	public class DispatcherService : IDispatcherService
	{
		#region Public Properties

		public Dispatcher Dispatcher
		{
			get
			{
				if (Application.Current == null ||
					Application.Current.Dispatcher == null)
				{
					throw new InvalidOperationException();
				}
				return Application.Current.Dispatcher;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public void Invoke(Action action)
		{
			Dispatcher.Invoke(action);
		}

		public TResult Invoke<TResult>(Func<TResult> func)
		{
			return Dispatcher.Invoke(func);
		}

		#endregion Public Methods
	}
}