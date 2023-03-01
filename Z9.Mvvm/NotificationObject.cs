using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Z9.Mvvm;

/// <summary>
/// ViewModel base class
/// </summary>
public abstract class NotificationObject : INotifyPropertyChanged
{
    readonly Dictionary<string, object> fieldList = new();

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
    public void RaisePropertyChanged([CallerMemberName] string propertyName = default) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// A method for property getter
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    /// <param name="propertyName">Property name</param>
    /// <returns>Property value</returns>
    /// <exception cref="ArgumentNullException"/>
    protected T GetValue<T>([CallerMemberName] string propertyName = default) =>
        GetValue(default(T), propertyName);

    /// <summary>
    /// A method for property getter
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    /// <param name="defaultValue">Default value set</param>
    /// <param name="propertyName">Property name</param>
    /// <returns>Property value</returns>
    /// <exception cref="ArgumentNullException"/>
    protected T GetValue<T>(T defaultValue, [CallerMemberName] string propertyName = default)
    {
        if (propertyName is null)
            throw new ArgumentNullException(nameof(propertyName));
        if (fieldList.TryGetValue(propertyName, out var value))
            return (T)value;
        fieldList.Add(propertyName, defaultValue);
        return defaultValue;
    }

    /// <summary>
    /// A method for property setter
    /// </summary>
    /// <param name="newValue">Property value</param>
    /// <param name="propertyName">Property name</param>
    /// <exception cref="ArgumentNullException"/>
    protected void SetValue(object newValue, Action changedCallback = default, [CallerMemberName] string propertyName = default)
    {
        if (propertyName is null)
            throw new ArgumentNullException(nameof(propertyName));
        if (fieldList.ContainsKey(propertyName))
        {
            if (Equals(newValue, fieldList[propertyName]))
                return;
            fieldList[propertyName] = newValue;
        }
        else
            fieldList.Add(propertyName, newValue);
        RaisePropertyChanged(propertyName);
        changedCallback?.Invoke();
    }

    /// <summary>
    /// Xaml designer's code
    /// </summary>
    protected virtual void OnInitializeInDesignMode() { }

    /// <summary>
    /// User's code
    /// </summary>
    protected virtual void OnInitializeInRuntime() { }

    static NotificationObject() => 
        IsInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
}
