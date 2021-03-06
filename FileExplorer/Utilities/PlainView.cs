﻿using System.Windows;
using System.Windows.Controls;

namespace FileExplorer.Utilities
{
	public class PlainView : ViewBase
	{
		#region Public Fields

		public static readonly DependencyProperty
		  ItemContainerStyleProperty =
		  ItemsControl.ItemContainerStyleProperty.AddOwner(typeof(PlainView));

		public static readonly DependencyProperty ItemTemplateProperty =
			ItemsControl.ItemTemplateProperty.AddOwner(typeof(PlainView));

		public static readonly DependencyProperty ItemsPanelProperty = ItemsControl.ItemsPanelProperty.AddOwner(typeof(PlainView));

		public static readonly DependencyProperty ItemWidthProperty =
			WrapPanel.ItemWidthProperty.AddOwner(typeof(PlainView));

		public static readonly DependencyProperty ItemHeightProperty =
			WrapPanel.ItemHeightProperty.AddOwner(typeof(PlainView));

		#endregion Public Fields

		#region Public Properties

		public Style ItemContainerStyle
		{
			get { return (Style)GetValue(ItemContainerStyleProperty); }
			set { SetValue(ItemContainerStyleProperty, value); }
		}

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		public ItemsPanelTemplate ItemsPanel
		{
			get => (ItemsPanelTemplate)GetValue(ItemsPanelProperty);
			set => SetValue(ItemsPanelProperty, value);
		}

		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}

		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		#endregion Public Properties

		#region Protected Properties

		protected override object DefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(GetType(), "PlainView");
			}
		}

		#endregion Protected Properties
	}
}