using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	internal class ViewTypeConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly ViewTypeConverter instance = new ViewTypeConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is ListView listView))
			{
				return null;
			}
			var type = listView.Name.Replace("Items", "").Replace("View", "");
			return listView.TryFindResource(type + "Icon");
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