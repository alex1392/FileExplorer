using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	internal class TimeConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly TimeConverter instance = new TimeConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DateTimeOffset dateTimeOffset)
			{
				return dateTimeOffset.DateTime.ToString("g");
			}
			return null;
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