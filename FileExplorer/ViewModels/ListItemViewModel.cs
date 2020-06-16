using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileExplorer.ViewModels
{
	public abstract class ListItemViewModel : INotifyPropertyChanged
	{
		private ImageSource icon;
		protected readonly IServiceProvider serviceProvider;
		protected readonly IDispatcherService dispatcherService;

		public ImageSource Icon => icon ?? (icon = GetIcon());

		private string path;

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

		protected abstract void SetItem();

		protected abstract ImageSource GetIcon();

		public event PropertyChangedEventHandler PropertyChanged;

		public ListItemViewModel(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public ListItemViewModel(IServiceProvider serviceProvider, IDispatcherService dispatcherService) : this(serviceProvider)
		{
			this.dispatcherService = dispatcherService;
		}
	}
}
