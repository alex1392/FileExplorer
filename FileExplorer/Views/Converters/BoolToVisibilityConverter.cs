using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly BoolToVisibilityConverter instance = new BoolToVisibilityConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? Visibility.Visible : Visibility.Collapsed;
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