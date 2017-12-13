using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Z9.Mvvm.Converter
{
	/// <summary>
	/// Inverse the bool value
	/// </summary>
	public class BooleanNegation : IValueConverter
	{
		/// <summary>
		/// Inverse the bool value
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>target value</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is bool propSource)
			{
				if (propSource)
					return false;
				else
					return true;
			}

			Debug.WriteLine("Source value type must be bool");
			return DependencyProperty.UnsetValue;
		}

		/// <summary>
		/// This converter not allowed to convert back
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>source value</returns>
		/// <exception cref="NotImplementedException"/>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}
