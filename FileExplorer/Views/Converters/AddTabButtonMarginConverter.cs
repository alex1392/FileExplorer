using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	public class AddTabButtonMarginConverter : MarkupExtension, IValueConverter
	{
		private static readonly AddTabButtonMarginConverter instance = new AddTabButtonMarginConverter();
		private double tabWidth = double.NaN;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is TabControl tabControl))
			{
				return null;
			}

			return new Thickness((tabControl.Items.Count - 1) * 112 + 116 + 3, 5.5, 0, 0);
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
