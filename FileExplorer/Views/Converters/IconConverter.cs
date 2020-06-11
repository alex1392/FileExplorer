using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace FileExplorer.Converters {
	/// <summary>
	/// Icon should be loaded dynamically in order to speed up application loading 
	/// </summary>
	public class IconConverter : MarkupExtension, IValueConverter {
		private static readonly IconConverter instance = new IconConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is FileItem fileItem) {
				var icon = Icon.ExtractAssociatedIcon(fileItem.Path);
				return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			} else if (value is TreeFolderItem folderItem) {
				return folderItem.Icon ?? new BitmapImage(new Uri(Path.Combine(App.PackUri, "Resources/Folder.ico")));
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
	}
}
