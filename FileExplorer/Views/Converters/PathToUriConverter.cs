using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Views.Converters
{
	public class PathToUriConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly PathToUriConverter instance = new PathToUriConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var path = value?.ToString();
			if (path == null)
			{
				return null;
			}
			var imageExt = new List<string> { ".jpg", ".jpeg", ".png", ".bmp", ".tif", ".svg" };
			// check if the path is an image
			if (imageExt.Contains(Path.GetExtension(path).ToLower()))
				return new Uri(path).AbsoluteUri;
			else
				return null;
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