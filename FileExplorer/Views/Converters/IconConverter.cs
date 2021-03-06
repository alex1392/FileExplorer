﻿using FileExplorer.Models;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace FileExplorer.Views.Converters
{
	/// <summary>
	/// Icon should be loaded dynamically in order to speed up application loading
	/// </summary>
	public class IconConverter : MarkupExtension, IValueConverter
	{
		#region Private Fields

		private static readonly IconConverter instance = new IconConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is ITreeItem treeItem))
			{
				return null;
			}
			if (treeItem.IconKey != null)
			{
				return new BitmapImage(new Uri($"/Resources/{treeItem.IconKey}.ico", UriKind.Relative));
			}
			else
			{
				return App.Current.TryFindResource("FolderIcon");
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