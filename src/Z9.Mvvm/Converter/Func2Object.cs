using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Z9.Mvvm.Converter
{
	/// <summary>
	/// Use delegate to create a converter, no more IValueConverter implement manually.
	/// </summary>
	public class Func2Object : IValueConverter
	{
		/// <summary>
		/// Convert Func defination
		/// </summary>
		public Func<object, object> Function { get; set; }

		/// <summary>
		/// ConvertBack Func defination
		/// </summary>
		public Func<object, object> FunctionBack { get; set; }

		/// <summary>
		/// Invoke the func
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>target value</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				return Function?.Invoke(value) ?? Binding.DoNothing;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Converter exception: Exception throwed from delegate method '{ex.Message}' Converter [{GetType()}], value type [{value?.GetType().FullName}]");
				return Binding.DoNothing;
			}
		}

		/// <summary>
		/// This converter not allowed to convert back
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>source value</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				return FunctionBack?.Invoke(value) ?? Binding.DoNothing;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Converter exception: Exception throwed from delegate method (ConvertBack) '{ex.Message}' Converter [{GetType()}], value type [{value?.GetType().FullName}]");
				return Binding.DoNothing;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public Func2Object() { }

		/// <summary>
		/// Construct with delegate
		/// </summary>
		/// <param name="func">Func</param>
		public Func2Object(Func<object, object> func) => Function = func;

		/// <summary>
		/// Construct with two delegate
		/// </summary>
		/// <param name="func"></param>
		/// <param name="funcBack"></param>
		public Func2Object(Func<object,object> func, Func<object,object> funcBack)
		{
			Function = func;
			FunctionBack = funcBack;
		}
	}
}
