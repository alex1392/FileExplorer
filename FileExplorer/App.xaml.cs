using FileExplorer.Models;
using FileExplorer.ViewModels;
using FileExplorer.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileExplorer {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public static readonly string PackUri = $"pack://application:,,,/{ResourceAssembly.GetName().Name};component/";

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var serviceCollection = new ServiceCollection();
			
			ConfigureServices(serviceCollection);

			var serviceProvider = serviceCollection.BuildServiceProvider();
			
			var mainWindow = serviceProvider.GetService<MainWindow>();
			mainWindow.Show();
		}

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
			serviceCollection.AddSingleton<IFolderNavigationService, FolderNavigationService>();
		}
	}
}
