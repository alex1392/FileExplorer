using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Converters {
	class SizeConverter : MarkupExtension, IValueConverter {
		private static readonly SizeConverter instance = new SizeConverter();
		private static readonly string[] Units = new string[]
		{
			"Byte", "KB", "MB", "GB", "TB"
		};
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double size = (long)value;
			var i = 0;
			while (size > 1024d) {
				size /= 1024d;
				i++;
			}
			return $"{string.Format("{0:0.##}", size)} {Units[i]}";
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
