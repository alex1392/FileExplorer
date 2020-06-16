using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	class ViewTypeConverter : MarkupExtension, IMultiValueConverter
	{
		private static readonly ViewTypeConverter instance = new ViewTypeConverter();
		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value[0] is ViewType viewType) || !(value[1] is Button button))
			{
				return null;
			}
			return viewType switch
			{
				ViewType.ListView => button.TryFindResource("ListIcon"),
				ViewType.GridView => button.TryFindResource("GridIcon"),
				ViewType.TileView => button.TryFindResource("TileIcon"),
				_ => null,
			};
		}

		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return instance;
		}
	}
}
