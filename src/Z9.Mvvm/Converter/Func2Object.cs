using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Z9.Mvvm.Converter
{
	/// <summary>
	/// Use Func() to convert
	/// </summary>
	public class Func2Object : IValueConverter
	{
		/// <summary>
		/// Func() defination
		/// </summary>
		public Func<object, object> Function { get; set; }

		/// <summary>
		/// Invoke the func
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>target value</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Function?.Invoke(value);

		/// <summary>
		/// This converter not allowed to convert back
		/// </summary>
		/// <param name="value">source value</param>
		/// <param name="targetType">target type</param>
		/// <param name="parameter">parameter</param>
		/// <param name="culture">culture info</param>
		/// <returns>source value</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

		/// <summary>
		/// Default constructor
		/// </summary>
		public Func2Object() { }

		/// <summary>
		/// Construct with delegate
		/// </summary>
		/// <param name="func">Func</param>
		public Func2Object(Func<object, object> func) => Function = func;
	}
}
