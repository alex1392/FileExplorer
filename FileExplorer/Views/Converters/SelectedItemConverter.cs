using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace FileExplorer.Views.Converters
{
	public class SelectedItemConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly SelectedItemConverter instance = new SelectedItemConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var collection = (value as ItemCollection).OfType<DependencyObject>();
			var current = collection.FirstOrDefault(obj => JournalEntryUnifiedViewConverter.GetJournalEntryPosition(obj) == JournalEntryPosition.Current);
			return current;
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