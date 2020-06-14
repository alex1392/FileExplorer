﻿using FileExplorer.Models;

using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace FileExplorer.Views.Converters {

	/// <summary>
	/// Icon should be loaded dynamically in order to speed up application loading
	/// </summary>
	public class IconConverter : MarkupExtension, IValueConverter {

		#region Private Fields

		private static readonly IconConverter instance = new IconConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ListFileItem fileItem) {
				var icon = Icon.ExtractAssociatedIcon(fileItem.Path);
				return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			} else if (value is ListFolderItem listFolderItem) {
				return new BitmapImage(new Uri("/Resources/Folder.ico", UriKind.Relative));
			} else if (value is TreeFolderItem treeFolderItem) {
				return new BitmapImage(new Uri($"/Resources/{treeFolderItem.IconKey ?? "Folder"}.ico", UriKind.Relative));
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

		#endregion Public Methods
	}
}