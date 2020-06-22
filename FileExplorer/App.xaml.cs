using FileExplorer.Models;
using FileExplorer.ViewModels;
using FileExplorer.Views;
using FileExplorer.Views.Services;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

namespace FileExplorer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
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
			serviceCollection.AddSingleton<IServiceProvider>(sp => sp);
			serviceCollection.AddSingleton<IFileProvider, FileProvider>();
			serviceCollection.AddSingleton<ISystemFolderProvider, SystemFolderProvider>();

			serviceCollection.AddSingleton<INavigationService, FolderNavigationService>();
			serviceCollection.AddSingleton<IDialogService, DialogService>();
			serviceCollection.AddSingleton<ITypeDescriptionProvider, TypeDescriptionProvider>();
			serviceCollection.AddSingleton<IDispatcherService, DispatcherService>();
			serviceCollection.AddTransient<IFileWatcherService, FileWatcherService>();

			serviceCollection.AddSingleton<MainWindowViewModel>();
			serviceCollection.AddSingleton<MainWindow>();

			serviceCollection.AddTransient<FolderPage>();
			serviceCollection.AddTransient<FolderPageViewModel>();
			serviceCollection.AddTransient<FolderChildrenProvider>();

			serviceCollection.AddTransient<Item>();
			serviceCollection.AddTransient<ListFileItem>();
			serviceCollection.AddTransient<ListFolderItem>();
			serviceCollection.AddTransient<TreeFolderItem>();

			serviceCollection.AddTransient<ListFileItemViewModel>();
			serviceCollection.AddTransient<ListFolderItemViewModel>();

			serviceCollection.AddTransient<TreePageItem>();
			serviceCollection.AddSingleton<FileDropHandler>();
			serviceCollection.AddSingleton<FileDragHandler>();
			serviceCollection.AddSingleton<UndoRedoManager>(sp => new UndoRedoManager(capacity: 10));
			serviceCollection.AddTransient<CutPasteCommand>();
			serviceCollection.AddTransient<CopyPasteCommand>();
			serviceCollection.AddTransient<CreateCommand>();
			serviceCollection.AddTransient<DeleteCommand>();

			serviceCollection.AddSingleton<GoBackCommand>();
			serviceCollection.AddSingleton<GoForwardCommand>();
			serviceCollection.AddSingleton<RefreshCommand>();
			serviceCollection.AddSingleton<GoUpCommand>();
			serviceCollection.AddSingleton<GoHomeCommand>();
			serviceCollection.AddSingleton<RenameFileCommand>();

			serviceCollection.AddSingleton<RenameDialogCommand>();
		}

		#endregion Private Methods
	}
}