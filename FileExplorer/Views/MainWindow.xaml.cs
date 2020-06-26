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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

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
			CollectionViewSource.GetDefaultView(mainTabControl.Items).CollectionChanged += TabControl_CollectionChanged;
			mainTabControl.Items.Add(serviceProvider.GetService<TabContentUserControl>());
			mainTabControl.SelectedIndex = 0;
		}

		private void TabControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			BindingOperations.GetBindingExpression(AddTabButton, FrameworkElement.MarginProperty).UpdateTarget();
		}



		#endregion Public Constructors

		#region Events



		#endregion Events

		private void CloseTabButton_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is Button button))
			{
				return;
			}
			var obj = button as DependencyObject;
			do
			{
				obj = VisualTreeHelper.GetParent(obj);
			} while (obj != null && !(obj is TabItem));
			if (!(obj is TabItem tabItem))
			{
				throw new InvalidOperationException();
			}
			var index = mainTabControl.ItemContainerGenerator.IndexFromContainer(tabItem);
			mainTabControl.Items.Remove(mainTabControl.Items[index]);
		}

		private void AddTabButton_Click(object sender, RoutedEventArgs e)
		{
			mainTabControl.Items.Add(serviceProvider.GetService<TabContentUserControl>());
			mainTabControl.SelectedIndex = mainTabControl.Items.Count - 1;
		}
	}
}