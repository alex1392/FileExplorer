using FileExplorer.Models;
using FileExplorer.ViewModels;
using FileExplorer.Views;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

namespace FileExplorer {

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		#region Public Fields

		public static readonly string PackUri = $"pack://application:,,,/{ResourceAssembly.GetName().Name};component/";

		#endregion Public Fields

		#region Protected Methods

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var serviceCollection = new ServiceCollection();

			ConfigureServices(serviceCollection);

			var serviceProvider = serviceCollection.BuildServiceProvider();

			var mainWindow = serviceProvider.GetService<MainWindow>();
			mainWindow.Show();
		}

		#endregion Protected Methods

		#region Private Methods

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IFileProvider, FileProvider>();
			serviceCollection.AddSingleton<ISystemFolderProvider, SystemFolderProvider>();
			serviceCollection.AddSingleton<MainWindowViewModel>();
			serviceCollection.AddSingleton<MainWindow>();

			serviceCollection.AddSingleton<IServiceProvider>(sp => sp);

			serviceCollection.AddTransient<FolderPage>();
			serviceCollection.AddTransient<FolderPageViewModel>();
			serviceCollection.AddTransient<FolderChildrenProvider>();
			serviceCollection.AddSingleton<INavigationService, NavigationService>();

			serviceCollection.AddSingleton<IDialogService, DialogService>();
			serviceCollection.AddSingleton<ITypeDescriptionProvider, TypeDescriptionProvider>();

			serviceCollection.AddTransient<Item>();
			serviceCollection.AddTransient<ListFileItem>();
			serviceCollection.AddTransient<ListFolderItem>();
			serviceCollection.AddTransient<TreeFolderItem>();
		}

		#endregion Private Methods
	}
}