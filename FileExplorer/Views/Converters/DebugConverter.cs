using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Converters {

	public class DebugConverter : MarkupExtension, IValueConverter {

		#region Private Fields

		private static readonly DebugConverter instance = new DebugConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debugger.Break();
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debugger.Break();
			return value;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return instance;
		}

		#endregion Public Methods
	}
}