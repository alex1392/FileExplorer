using FileExplorer.Models;
using FileExplorer.Views;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using IO = System.IO;

namespace FileExplorer.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private readonly IServiceProvider serviceProvider;

		public event PropertyChangedEventHandler PropertyChanged;


		public MainWindowViewModel()
		{

		}

		public MainWindowViewModel(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;

		}
	}
}