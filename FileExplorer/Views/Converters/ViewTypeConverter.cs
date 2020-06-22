using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	internal class ViewTypeConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly ViewTypeConverter instance = new ViewTypeConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var type = value?.ToString().Replace("View", "");
			return App.Current.MainWindow.TryFindResource(type + "Icon");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return instance;
		}

		#endregion Public Methods
	}
}