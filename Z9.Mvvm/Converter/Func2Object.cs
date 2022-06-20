using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Z9.Mvvm.Converter;

/// <summary>
/// Use delegate to create a converter, no more IValueConverter implement manually.
/// </summary>
public class Func2Object : IValueConverter
{
    Func<object, object> function;
    Func<object, object> functionBack;

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
            return function?.Invoke(value) ?? Binding.DoNothing;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Converter exception: Exception throwed from delegate method '{ex.Message}' Converter [{GetType()}], value type [{value?.GetType().FullName}]");
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// Invoke the back func
    /// </summary>
    /// <param name="value">target value</param>
    /// <param name="targetType">target type</param>
    /// <param name="parameter">parameter</param>
    /// <param name="culture">culture info</param>
    /// <returns>source value</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            return functionBack?.Invoke(value) ?? Binding.DoNothing;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Converter exception: Exception throwed from delegate method (ConvertBack) '{ex.Message}' Converter [{GetType()}], value type [{value?.GetType().FullName}]");
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// Construct with delegate
    /// </summary>
    /// <param name="func">Func</param>
    public Func2Object(Func<object, object> func) => 
        function = func;

    /// <summary>
    /// Construct with two delegate
    /// </summary>
    /// <param name="func"></param>
    /// <param name="funcBack"></param>
    public Func2Object(Func<object, object> func, Func<object, object> funcBack)
    {
        function = func;
        functionBack = funcBack;
    }
}
