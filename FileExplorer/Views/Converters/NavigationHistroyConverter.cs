using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace FileExplorer.Views.Converters
{
	public class NavigationHistroyConverter : MarkupExtension, IMultiValueConverter
	{
		#region Private Fields

		private static readonly NavigationHistroyConverter instance = new NavigationHistroyConverter();

		#endregion Private Fields

		#region Public Methods
		/// <summary>
		/// TODO: cannot use priority binding??
		/// </summary>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return values[0] != DependencyProperty.UnsetValue 
				? values[0]
				: values[1];
		}


		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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