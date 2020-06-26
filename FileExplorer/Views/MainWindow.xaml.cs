using Cyc.FluentDesign;

using FileExplorer.Models;
using FileExplorer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
		private readonly IServiceProvider serviceProvider;
		#endregion Private Fields

		#region Private Properties


		#endregion Private Properties

		public object CurrentTab => mainTabControl.SelectedItem;

		#region Public Constructors

		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		public MainWindow(MainWindowViewModel vm, IServiceProvider serviceProvider) : this()
		{
			this.vm = vm;
			this.serviceProvider = serviceProvider;
			DataContext = this.vm;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			mainTabControl.Items.Add(serviceProvider.GetService<TabContentUserControl>());
			mainTabControl.SelectedIndex = 0;
		}

		#endregion Public Constructors



		#region Events



		#endregion Events

	}
}