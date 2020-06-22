using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileExplorer.Utilities
{
	public class PlainView : ViewBase
	{

		public static readonly DependencyProperty
		  ItemContainerStyleProperty =
		  ItemsControl.ItemContainerStyleProperty.AddOwner(typeof(PlainView));

		public Style ItemContainerStyle
		{
			get { return (Style)GetValue(ItemContainerStyleProperty); }
			set { SetValue(ItemContainerStyleProperty, value); }
		}

		public static readonly DependencyProperty ItemTemplateProperty =
			ItemsControl.ItemTemplateProperty.AddOwner(typeof(PlainView));

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		public static readonly DependencyProperty ItemsPanelProperty = ItemsControl.ItemsPanelProperty.AddOwner(typeof(PlainView));

		public ItemsPanelTemplate ItemsPanel
		{
			get => (ItemsPanelTemplate)GetValue(ItemsPanelProperty);
			set => SetValue(ItemsPanelProperty, value);
		}

		public static readonly DependencyProperty ItemWidthProperty =
			WrapPanel.ItemWidthProperty.AddOwner(typeof(PlainView));

		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}

		public static readonly DependencyProperty ItemHeightProperty =
			WrapPanel.ItemHeightProperty.AddOwner(typeof(PlainView));

		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		protected override object DefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(GetType(), "PlainView");
			}
		}
	}
}
