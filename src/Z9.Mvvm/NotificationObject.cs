using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace Z9.Mvvm
{
	/// <summary>
	/// ViewModel base class
	/// </summary>
	public abstract class NotificationObject : INotifyPropertyChanged
	{
		readonly Dictionary<string, object> propList = new Dictionary<string, object>();

		/// <summary>
		/// Occurs when property value changed
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Current code mode (design mode | run-time mode)
		/// </summary>
		public static bool IsInDesignMode { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		public NotificationObject()
		{
			if (IsInDesignMode)
				OnInitializeInDesignMode();
			else
				OnInitializeInRuntime();
		}

		/// <summary>
		/// Notify property changed
		/// </summary>
		/// <param name="propertyName">Property name</param>
		public void OnPropertyChanged([CallerMemberName]string propertyName = default) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// Notify all properties changed(beta)
		/// </summary>
		public void NotifyAll() => Application.Current?.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, null));

		/// <summary>
		/// Set property value
		/// </summary>
		/// <typeparam name="T">Property type</typeparam>
		/// <param name="oldValue">Old value</param>
		/// <param name="newValue">New value</param>
		/// <param name="propertyName">Property name</param>
		/// <returns>Whether success</returns>
		protected bool Set<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = default)
		{
			if (newValue != null && newValue.Equals(oldValue))
				return false;
			oldValue = newValue;
			OnPropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// A method for property getter
		/// </summary>
		/// <typeparam name="T">Property type</typeparam>
		/// <param name="propertyName">Property name</param>
		/// <returns>Property value</returns>
		/// <exception cref="InvalidOperationException"/>
		protected T GetProperty<T>([CallerMemberName]string propertyName = default) => GetProperty(default(T), propertyName);

		/// <summary>
		/// A method for property getter
		/// </summary>
		/// <typeparam name="T">Property type</typeparam>
		/// <param name="defaultValue">Default value set</param>
		/// <param name="propertyName">Property name</param>
		/// <returns>Property value</returns>
		/// <exception cref="InvalidOperationException"/>
		protected T GetProperty<T>(T defaultValue, [CallerMemberName]string propertyName = default)
		{
			if (propertyName == default)
				throw new InvalidOperationException();
			if (propList.ContainsKey(propertyName))
				return (T)propList[propertyName];
			propList.Add(propertyName, defaultValue);
			return defaultValue;
		}

		/// <summary>
		/// A method for property setter
		/// </summary>
		/// <param name="propertyValue">Property value</param>
		/// <param name="propertyName">Property name</param>
		/// <exception cref="InvalidOperationException"/>
		protected void SetProperty(object propertyValue, [CallerMemberName]string propertyName = default)
		{
			if (propertyName == default)
				throw new InvalidOperationException();
			if (propList.ContainsKey(propertyName))
			{
				if (propertyValue != null && propertyValue.Equals(propList[propertyName]))
					return;
				propList[propertyName] = propertyValue;
				OnPropertyChanged(propertyName);
				return;
			}
			propList.Add(propertyName, propertyValue);
			OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Xaml designer's code
		/// </summary>
		protected virtual void OnInitializeInDesignMode() { }

		/// <summary>
		/// User's code
		/// </summary>
		protected virtual void OnInitializeInRuntime() { }

		static NotificationObject()
		{
			var prop = DesignerProperties.IsInDesignModeProperty;
			IsInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
		}
	}
}
