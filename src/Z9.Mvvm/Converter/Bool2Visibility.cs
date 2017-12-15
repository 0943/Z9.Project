using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Z9.Mvvm.Converter
{
	/// <summary>
	/// An extended version of the standard converter that maps Boolean values to the values of the Visibility type and vice versa
	/// </summary>
	public sealed class Bool2Visibility : IValueConverter
	{
		/// <summary>
		/// Use Hidden rather than Collapsed
		/// </summary>
		public bool UseHiddenInstead { get; set; }

		/// <summary>
		/// Inverse the boolean value
		/// </summary>
		public bool Inversed { get; set; }

		/// <summary>
		/// Convert from source value type to target type
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>target value</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!bool.TryParse(value?.ToString(), out bool propSource))
			{
				Debug.WriteLine($"Converter Exception: Source value type must be Boolean. Converter [{GetType()}], value type [{value?.GetType().FullName}]");
				return Binding.DoNothing;
			}
			if (Inversed)
			{
				if (propSource)
				{
					if (UseHiddenInstead)
						return Visibility.Hidden;
					else
						return Visibility.Collapsed;
				}
				else
					return Visibility.Visible;
			}
			else
			{
				if (propSource)
					return Visibility.Visible;
				else
				{
					if (UseHiddenInstead)
						return Visibility.Hidden;
					else
						return Visibility.Collapsed;
				}
			}
		}

		/// <summary>
		/// Convert from target value type to source type
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>source value</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(!(value is Visibility visible))
			{
				Debug.WriteLine($"Converter Exception: Target value type must be Visibility. Converter [{GetType()}], value type [{value?.GetType().FullName}]");
				return Binding.DoNothing;
			}
			if (visible == Visibility.Visible)
			{
				if(Inversed)
					return false;
				else
					return true;
			}
			else
			{
				if (Inversed)
					return true;
				else
					return false;
			}
		}
	}
}