﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace FileExplorer.Views {

	public class DropDownButtonBehavior : Behavior<Button> {
		private bool isContextMenuOpen;

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), true);
		}

		void AssociatedObject_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button source && source.ContextMenu != null) {
				if (!isContextMenuOpen) {
					// Add handler to detect when the ContextMenu closes
					source.ContextMenu.AddHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed), true);
					// If there is a drop-down assigned to this button, then position and display it 
					source.ContextMenu.PlacementTarget = source;
					source.ContextMenu.Placement = PlacementMode.Bottom;
					source.ContextMenu.IsOpen = true;
					isContextMenuOpen = true;
				}
			}
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
		}

		void ContextMenu_Closed(object sender, RoutedEventArgs e)
		{
			isContextMenuOpen = false;
			if (sender is ContextMenu contextMenu) {
				contextMenu.RemoveHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed));
			}
		}
	}
}