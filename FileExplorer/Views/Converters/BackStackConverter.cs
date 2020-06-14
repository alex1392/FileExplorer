using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace FileExplorer.Converters {
	public class BackStackConverter : MarkupExtension, IValueConverter {
		private static readonly BackStackConverter instance = new BackStackConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as IEnumerable).Cast<JournalEntry>().Select(entry => entry.Name);
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
