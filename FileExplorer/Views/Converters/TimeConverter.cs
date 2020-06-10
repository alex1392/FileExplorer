using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Converters {
	class TimeConverter : MarkupExtension, IValueConverter {
		private static readonly TimeConverter instance = new TimeConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DateTimeOffset dateTimeOffset) {
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
	}
}
