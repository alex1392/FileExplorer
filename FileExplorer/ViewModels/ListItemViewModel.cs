using FileExplorer.Models;

using System;
using System.ComponentModel;
using System.Windows.Media;

namespace FileExplorer.ViewModels
{
	public abstract class ListItemViewModel : INotifyPropertyChanged
	{
		#region Protected Fields

		protected readonly IServiceProvider serviceProvider;
		protected readonly IDispatcherService dispatcherService;

		#endregion Protected Fields

		#region Private Fields

		private ImageSource icon;
		private string path;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public ImageSource Icon => icon ?? (icon = GetIcon());

		public string Path
		{
			get { return path; }
			set
			{
				if (path != null || value == path)
				{
					return;
				}
				path = value;

				SetItem();

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));
			}
		}

		public ListItem Item { get; protected set; }

		#endregion Public Properties

		#region Public Constructors

		public ListItemViewModel(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public ListItemViewModel(IServiceProvider serviceProvider, IDispatcherService dispatcherService) : this(serviceProvider)
		{
			this.dispatcherService = dispatcherService;
		}

		#endregion Public Constructors

		#region Protected Methods

		protected abstract void SetItem();

		protected abstract ImageSource GetIcon();

		#endregion Protected Methods
	}
}