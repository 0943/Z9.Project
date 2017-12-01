using System;
using System.Globalization;
using System.Windows.Data;

namespace Z9.Mvvm.Converter
{
	/// <summary>
	/// Converts the input Boolean, nullable Boolean or DefaultBoolean value to a value of any type
	/// </summary>
	public sealed class Bool2Object : IValueConverter
	{
		/// <summary>
		/// true value
		/// </summary>
		public object TrueValue { get; set; }

		/// <summary>
		/// false value
		/// </summary>
		public object FalseValue { get; set; }

		/// <summary>
		/// null value
		/// </summary>
		public object NullValue { get; set; }

		/// <summary>
		/// Convert from source value type to target type
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>target value</returns>
		/// /// <exception cref="InvalidCastException">Source value type is not bool or bool?</exception>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (bool.TryParse(value?.ToString(), out bool propSource))
			{
				if (propSource)
					return TrueValue;
				else
					return FalseValue;
			}
			try
			{
				var propSource2 = (bool?)value;
				return NullValue;
			}
			catch { throw; }
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
