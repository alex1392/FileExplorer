using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Converters {

	internal class SizeConverter : MarkupExtension, IValueConverter {

		#region Private Fields

		private static readonly SizeConverter instance = new SizeConverter();

		private static readonly string[] Units = new string[]
		{
			"Byte", "KB", "MB", "GB", "TB"
		};

		#endregion Private Fields

		#region Public Methods

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

		#endregion Public Methods
	}
}