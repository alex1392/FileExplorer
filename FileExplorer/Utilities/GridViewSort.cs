﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FileExplorer.Utilities
{
	public class GridViewSort
	{
		#region Private Classes

		private class SortGlyphAdorner : Adorner
		{
			#region Private Fields

			private readonly GridViewColumnHeader _columnHeader;
			private readonly ListSortDirection _direction;
			private readonly ImageSource _sortGlyph;

			#endregion Private Fields

			#region Public Constructors

			public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
				: base(columnHeader)
			{
				_columnHeader = columnHeader;
				_direction = direction;
				_sortGlyph = sortGlyph;
			}

			#endregion Public Constructors

			#region Protected Methods

			protected override void OnRender(DrawingContext drawingContext)
			{
				base.OnRender(drawingContext);

				if (_sortGlyph != null)
				{
					var x = _columnHeader.ActualWidth - 13;
					var y = _columnHeader.ActualHeight / 2 - 5;
					var rect = new Rect(x, y, 10, 10);
					drawingContext.DrawImage(_sortGlyph, rect);
				}
				else
				{
					drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), GetDefaultGlyph());
				}
			}

			#endregion Protected Methods

			#region Private Methods

			private Geometry GetDefaultGlyph()
			{
				var x1 = _columnHeader.ActualWidth - 13;
				var x2 = x1 + 10;
				var x3 = x1 + 5;
				var y1 = _columnHeader.ActualHeight / 2 - 3;
				var y2 = y1 + 5;

				if (_direction == ListSortDirection.Ascending)
				{
					var tmp = y1;
					y1 = y2;
					y2 = tmp;
				}

				var pathSegmentCollection = new PathSegmentCollection();
				pathSegmentCollection.Add(new LineSegment(new Point(x2, y1), true));
				pathSegmentCollection.Add(new LineSegment(new Point(x3, y2), true));

				var pathFigure = new PathFigure(
					new Point(x1, y1),
					pathSegmentCollection,
					true);

				var pathFigureCollection = new PathFigureCollection();
				pathFigureCollection.Add(pathFigure);

				var pathGeometry = new PathGeometry(pathFigureCollection);
				return pathGeometry;
			}

			#endregion Private Methods
		}

		#endregion Private Classes

		#region Public Fields

		// Using a DependencyProperty as the backing store for AutoSort.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AutoSortProperty =
			DependencyProperty.RegisterAttached(
				"AutoSort",
				typeof(bool),
				typeof(GridViewSort),
				new UIPropertyMetadata(
					false,
					(o, e) =>
					{
						var listView = o as ListView;
						if (listView != null)
						{
							if (GetCommand(listView) == null) // Don't change click handler if a command is set
							{
								var oldValue = (bool)e.OldValue;
								var newValue = (bool)e.NewValue;
								if (oldValue && !newValue)
								{
									listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
								}
								if (!oldValue && newValue)
								{
									listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
								}
							}
						}
					}
				)
			);

		// Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached(
				"Command",
				typeof(ICommand),
				typeof(GridViewSort),
				new UIPropertyMetadata(
					null,
					(o, e) =>
					{
						var listView = o as ItemsControl;
						if (listView != null)
						{
							if (!GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
							{
								if (e.OldValue != null && e.NewValue == null)
								{
									listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
								}
								if (e.OldValue == null && e.NewValue != null)
								{
									listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
								}
							}
						}
					}
				)
			);

		// Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PropertyNameProperty =
			DependencyProperty.RegisterAttached(
				"PropertyName",
				typeof(string),
				typeof(GridViewSort),
				new UIPropertyMetadata(null)
			);

		// Using a DependencyProperty as the backing store for ShowSortGlyph.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ShowSortGlyphProperty =
			DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(GridViewSort), new UIPropertyMetadata(true));

		// Using a DependencyProperty as the backing store for SortGlyphAscending.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SortGlyphAscendingProperty =
			DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

		// Using a DependencyProperty as the backing store for SortGlyphDescending.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SortGlyphDescendingProperty =
			DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

		#endregion Public Fields

		#region Private Fields

		// Using a DependencyProperty as the backing store for SortedColumn.  This enables animation, styling, binding, etc...
		private static readonly DependencyProperty SortedColumnHeaderProperty =
			DependencyProperty.RegisterAttached("SortedColumnHeader", typeof(GridViewColumnHeader), typeof(GridViewSort), new UIPropertyMetadata(null));

		#endregion Private Fields

		#region Public Methods

		public static void ApplySort(ICollectionView view, string propertyName, ListView listView, GridViewColumnHeader sortedColumnHeader)
		{
			var direction = ListSortDirection.Ascending;
			if (view.SortDescriptions.Count > 0)
			{
				var currentSort = view.SortDescriptions[0];
				if (currentSort.PropertyName == propertyName)
				{
					if (currentSort.Direction == ListSortDirection.Ascending)
						direction = ListSortDirection.Descending;
					else
						direction = ListSortDirection.Ascending;
				}
				view.SortDescriptions.Clear();

				var currentSortedColumnHeader = GetSortedColumnHeader(listView);
				if (currentSortedColumnHeader != null)
				{
					RemoveSortGlyph(currentSortedColumnHeader);
				}
			}
			if (!string.IsNullOrEmpty(propertyName))
			{
				view.SortDescriptions.Add(new SortDescription(propertyName, direction));
				if (GetShowSortGlyph(listView))
					AddSortGlyph(
						sortedColumnHeader,
						direction,
						direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
				SetSortedColumnHeader(listView, sortedColumnHeader);
			}
		}

		public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
		{
			var parent = VisualTreeHelper.GetParent(reference);
			while (!(parent is T))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}
			if (parent != null)
				return (T)parent;
			else
				return null;
		}

		public static bool GetAutoSort(DependencyObject obj)
		{
			return (bool)obj.GetValue(AutoSortProperty);
		}

		public static ICommand GetCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(CommandProperty);
		}

		public static string GetPropertyName(DependencyObject obj)
		{
			return (string)obj.GetValue(PropertyNameProperty);
		}

		public static bool GetShowSortGlyph(DependencyObject obj)
		{
			return (bool)obj.GetValue(ShowSortGlyphProperty);
		}

		public static ImageSource GetSortGlyphAscending(DependencyObject obj)
		{
			return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
		}

		public static ImageSource GetSortGlyphDescending(DependencyObject obj)
		{
			return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
		}

		public static void SetAutoSort(DependencyObject obj, bool value)
		{
			obj.SetValue(AutoSortProperty, value);
		}

		public static void SetCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(CommandProperty, value);
		}

		public static void SetPropertyName(DependencyObject obj, string value)
		{
			obj.SetValue(PropertyNameProperty, value);
		}

		public static void SetShowSortGlyph(DependencyObject obj, bool value)
		{
			obj.SetValue(ShowSortGlyphProperty, value);
		}

		public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
		{
			obj.SetValue(SortGlyphAscendingProperty, value);
		}

		public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
		{
			obj.SetValue(SortGlyphDescendingProperty, value);
		}

		#endregion Public Methods

		#region Private Methods

		private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
		{
			var adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
			adornerLayer.Add(
				new SortGlyphAdorner(
					columnHeader,
					direction,
					sortGlyph
					));
		}

		private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			var headerClicked = e.OriginalSource as GridViewColumnHeader;
			if (headerClicked != null && headerClicked.Column != null)
			{
				var propertyName = GetPropertyName(headerClicked.Column);
				if (!string.IsNullOrEmpty(propertyName))
				{
					var listView = GetAncestor<ListView>(headerClicked);
					if (listView != null)
					{
						var command = GetCommand(listView);
						if (command != null)
						{
							if (command.CanExecute(propertyName))
							{
								command.Execute(propertyName);
							}
						}
						else if (GetAutoSort(listView))
						{
							ApplySort(listView.Items, propertyName, listView, headerClicked);
						}
					}
				}
			}
		}

		private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
		{
			return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
		}

		private static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
		{
			var adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
			var adorners = adornerLayer.GetAdorners(columnHeader);
			if (adorners != null)
			{
				foreach (var adorner in adorners)
				{
					if (adorner is SortGlyphAdorner)
						adornerLayer.Remove(adorner);
				}
			}
		}

		private static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
		{
			obj.SetValue(SortedColumnHeaderProperty, value);
		}

		#endregion Private Methods
	}
}