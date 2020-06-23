using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	public class PathToUriConverter : MarkupExtension, IValueConverter
	{
		private static readonly PathToUriConverter instance = new PathToUriConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var path = value?.ToString();
			if (path == null)
			{
				return null;
			}
			return new Uri(path).AbsoluteUri;
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
