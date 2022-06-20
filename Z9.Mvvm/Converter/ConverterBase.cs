using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Z9.Mvvm.Converter;

/// <summary>
/// Base converter for supporting create instance in xaml.
/// </summary>
public abstract class ConverterBase : MarkupExtension, IValueConverter
{
	/// <summary>
	/// Convert from source value type to target type
	/// </summary>
	/// <param name="value">source value</param>
	/// <param name="targetType">target type</param>
	/// <param name="parameter">parameter</param>
	/// <param name="culture">culture info</param>
	/// <returns>target value</returns>
	public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

	/// <summary>
	/// Convert from target value type to source type
	/// </summary>
	/// <param name="value">target value</param>
	/// <param name="targetType">target type</param>
	/// <param name="parameter">parameter</param>
	/// <param name="culture">culture info</param>
	/// <returns>source value</returns>
	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		Binding.DoNothing;

	/// <summary>
	/// Instance of markcup object, default is 'this'
	/// </summary>
	/// <param name="serviceProvider">Service provider</param>
	/// <returns>Markcup object value</returns>
	public override object ProvideValue(IServiceProvider serviceProvider) => 
		this;
}
