using Cyc.FluentDesign;

using FileExplorer.Models;
using FileExplorer.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileExplorer.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RevealWindow
	{
		#region Private Fields

		private readonly MainWindowViewModel vm;
		#endregion Private Fields

		#region Private Properties


		#endregion Private Properties

		public object CurrentTab => mainTabControl.SelectedItem;

		#region Public Constructors

		public MainWindow()
		{
			InitializeComponent();
		}

		public MainWindow(MainWindowViewModel vm) : this()
		{
			this.vm = vm;
			DataContext = this.vm;
		}

		#endregion Public Constructors

		

		#region Events



		#endregion Events

	}
}