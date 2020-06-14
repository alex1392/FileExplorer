using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace FileExplorer.Views.Converters {

	public class NavigationHistroyConverter : MarkupExtension, IValueConverter {

		#region Private Fields

		private static readonly NavigationHistroyConverter instance = new NavigationHistroyConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is JournalEntry entry) {
				return entry.Name;
			} else if (value is Page page) {
				return page.Title;
			} else {
				return null;
			}
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