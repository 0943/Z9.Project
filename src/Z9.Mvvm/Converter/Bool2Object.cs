using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Z9.Mvvm.Converter
{
	/// <summary>
	/// Converts the input Boolean, nullable Boolean or DefaultBoolean value to a value of any type
	/// </summary>
	[MarkupExtensionReturnType(typeof(Bool2Object))]
	public sealed class Bool2Object : ConverterBase
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
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
			catch (Exception ex)
			{
				Debug.WriteLine($"Converter exception: {ex.Message} Converter [{GetType()}], value type [{value?.GetType().FullName}]");
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
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine($"Converter exception: This converter doesn't support 'ConverBack'. Converter [{GetType()}], value type [{value?.GetType().FullName}]");
			return Binding.DoNothing;
		}
	}
}
