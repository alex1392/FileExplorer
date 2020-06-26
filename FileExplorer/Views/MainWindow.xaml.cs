using Cyc.FluentDesign;
using FileExplorer.Models;
using FileExplorer.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
		private readonly INavigationService navigationService;

		#endregion Private Fields



		#region Public Properties

		public object CurrentTab => mainTabControl.SelectedItem;

		#endregion Public Properties

		#region Public Constructors

		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		public MainWindow(MainWindowViewModel vm, IServiceProvider serviceProvider, INavigationService navigationService) : this()
		{
			this.vm = vm;
			this.serviceProvider = serviceProvider;
			this.navigationService = navigationService;
			DataContext = this.vm;

			navigationService.GoBackCompleted += (_, _) =>
				navigationService.Refresh();
			navigationService.GoForwardCompleted += (_, _) => 
				navigationService.Refresh();
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



		#region Private Methods

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

		#endregion Private Methods
	}
}