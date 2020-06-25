using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	public class FlowDocumentConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly FlowDocumentConverter instance = new FlowDocumentConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var path = value?.ToString();
			if (path == null)
			{
				return null;
			}
			if (Path.GetExtension(path).ToLower() != ".txt")
			{
				return null;
			}
			try
			{
				var text = File.ReadAllText(path);
				var paragraph = new Paragraph();
				paragraph.Inlines.Add(text);
				var doc = new FlowDocument(paragraph);
				return doc;
			}
			catch (Exception)
			{
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