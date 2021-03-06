﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace FileExplorer.DataVirtualization
{
	/// <summary>
	/// Derived VirtualizatingCollection, performing loading asychronously.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	public class AsyncVirtualizingCollection<T> : VirtualizingCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		#region Private Fields

		private readonly SynchronizationContext _synchronizationContext;

		private bool _isLoading;

		#endregion Private Fields

		#region Public Events

		/// <summary>
		/// Occurs when the collection changes.
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether the collection is loading.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this collection is loading; otherwise, <c>false</c>.
		/// </value>
		public bool IsLoading
		{
			get
			{
				return _isLoading;
			}
			set
			{
				if (value != _isLoading)
				{
					_isLoading = value;
				}
				FirePropertyChanged("IsLoading");
			}
		}

		#endregion Public Properties

		#region Protected Properties

		/// <summary>
		/// Gets the synchronization context used for UI-related operations. This is obtained as
		/// the current SynchronizationContext when the AsyncVirtualizingCollection is created.
		/// </summary>
		/// <value>The synchronization context.</value>
		protected SynchronizationContext SynchronizationContext
		{
			get { return _synchronizationContext; }
		}

		#endregion Protected Properties

		#region Public Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncVirtualizingCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider)
		: base(itemsProvider)
		{
			_synchronizationContext = SynchronizationContext.Current;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncVirtualizingCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		/// <param name="pageSize">Size of the page.</param>
		public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize)
			: base(itemsProvider, pageSize)
		{
			_synchronizationContext = SynchronizationContext.Current;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncVirtualizingCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="pageTimeout">The page timeout.</param>
		public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
			: base(itemsProvider, pageSize, pageTimeout)
		{
			_synchronizationContext = SynchronizationContext.Current;
		}

		#endregion Public Constructors

		#region Protected Methods

		/// <summary>
		/// Asynchronously loads the count of items.
		/// </summary>
		protected override void LoadCount()
		{
			Count = 0;
			IsLoading = true;
			ThreadPool.QueueUserWorkItem(LoadCountWork);
		}

		/// <summary>
		/// Asynchronously loads the page.
		/// </summary>
		/// <param name="index">The index.</param>
		protected override void LoadPage(int index)
		{
			IsLoading = true;
			ThreadPool.QueueUserWorkItem(LoadPageWork, index);
		}

		/// <summary>
		/// Raises the <see cref="E:CollectionChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			var h = CollectionChanged;
			if (h != null)
				h(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:PropertyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var h = PropertyChanged;
			if (h != null)
				h(this, e);
		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>
		/// Fires the collection reset event.
		/// </summary>
		private void FireCollectionReset()
		{
			var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			OnCollectionChanged(e);
		}

		/// <summary>
		/// Fires the property changed event.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		private void FirePropertyChanged(string propertyName)
		{
			var e = new PropertyChangedEventArgs(propertyName);
			OnPropertyChanged(e);
		}

		/// <summary>
		/// Performed on UI-thread after LoadCountWork.
		/// </summary>
		/// <param name="args">Number of items returned.</param>
		private void LoadCountCompleted(object args)
		{
			Count = (int)args;
			IsLoading = false;
			FireCollectionReset();
		}

		/// <summary>
		/// Performed on background thread.
		/// </summary>
		/// <param name="args">None required.</param>
		private void LoadCountWork(object args)
		{
			var count = FetchCount();
			SynchronizationContext.Send(LoadCountCompleted, count);
		}

		/// <summary>
		/// Performed on UI-thread after LoadPageWork.
		/// </summary>
		/// <param name="args">object[] { int pageIndex, IList(T) page }</param>
		private void LoadPageCompleted(object args)
		{
			var pageIndex = (int)((object[])args)[0];
			var page = (IList<T>)((object[])args)[1];

			PopulatePage(pageIndex, page);
			IsLoading = false;
			FireCollectionReset();
		}

		/// <summary>
		/// Performed on background thread.
		/// </summary>
		/// <param name="args">Index of the page to load.</param>
		private void LoadPageWork(object args)
		{
			var pageIndex = (int)args;
			var page = FetchPage(pageIndex);
			SynchronizationContext.Send(LoadPageCompleted, new object[] { pageIndex, page });
		}

		#endregion Private Methods
	}
}