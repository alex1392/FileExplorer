using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	public class FlowDocumentConverter : MarkupExtension, IValueConverter
	{
		private static readonly FlowDocumentConverter instance = new FlowDocumentConverter();
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
			var text = File.ReadAllText(path);
			var paragraph = new Paragraph();
			paragraph.Inlines.Add(text);
			var doc = new FlowDocument(paragraph);
			return doc;
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
