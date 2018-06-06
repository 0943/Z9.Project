using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Z9.Mvvm.Converter
{
	/// <summary>
	/// Inverse the bool value
	/// </summary>
	[MarkupExtensionReturnType(typeof(bool))]
	public class BooleanNegation : ConverterBase
	{
		/// <summary>
		/// Inverse the bool value
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>target value</returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is bool propSource)
			{
				if (propSource)
					return false;
				else
					return true;
			}

			Debug.WriteLine($"Converter exception: Source value type must be bool. Converter [{GetType()}], value type [{value?.GetType().FullName}]");
			return Binding.DoNothing;
		}

		/// <summary>
		/// Inverse the bool value to source
		/// </summary>
		/// <param name="value">target value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>source value</returns>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is bool propTarget)
			{
				if (propTarget)
					return false;
				else
					return true;
			}

			Debug.WriteLine($"Converter exception: Target value type must be bool. Converter [{GetType()}], value type [{value?.GetType().FullName}]");
			return Binding.DoNothing;
		}
	}
}
