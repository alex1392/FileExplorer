using FileExplorer.Models;

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
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<object> TabItems { get; private set; } = new ObservableCollection<object>();

		public MainWindowViewModel()
		{
			SetupTabItem();

			void SetupTabItem()
			{

			}
		}
	}
}