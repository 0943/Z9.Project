using System;
using System.Windows.Input;

namespace Z9.Mvvm.Command;

/// <summary>
/// Mvvm command tool
/// </summary>
public class DelegateCommand : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
    Action excuteAction;
    Func<bool> canExcuteFunc;

    /// <summary>
    /// Create command instance
    /// </summary>
    /// <param name="excute">The execution logic</param>
    /// <param name="canExcute">The execution status logic</param>
    public DelegateCommand(Action excute, Func<bool> canExcute = null)
    {
        excuteAction = excute;
        canExcuteFunc = canExcute;
    }

    /// <summary>
    /// Define a method that determines whether the command can execute
    /// </summary>
    /// <param name="parameter">This parameter will be always ignored</param>
    /// <returns>true</returns>
    public bool CanExecute(object parameter) =>
        canExcuteFunc == null || canExcuteFunc();

    /// <summary>
    /// Define a method to be called when command is invoked
    /// </summary>
    /// <param name="parameter">This parameter will be always triggered</param>
    public void Execute(object parameter) =>
        excuteAction?.Invoke();
}

/// <summary>
/// Mvvm command tool with parameter
/// </summary>
/// <typeparam name="T">CommandParameter Type</typeparam>
public class DelegateCommand<T> : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
    Action<T> excuteAction;
    Func<T, bool> canExcuteFunc;

    /// <summary>
    /// Create command instance
    /// </summary>
    /// <param name="excute">The execution logic</param>
    /// <param name="canExcute">The execution status logic</param>
    public DelegateCommand(Action<T> excute, Func<T, bool> canExcute = null)
    {
        excuteAction = excute;
        canExcuteFunc = canExcute;
    }

    /// <summary>
    /// Define a method that determines whether the command can execute
    /// </summary>
    /// <param name="parameter">Para</param>
    /// <returns>Whether executable</returns>
    public bool CanExecute(object parameter) =>
        canExcuteFunc == null || canExcuteFunc((T)Convert.ChangeType(parameter, typeof(T)));

    /// <summary>
    /// Define a method to be called when command is triggered
    /// </summary>
    /// <param name="parameter">Para</param>
    public void Execute(object parameter) =>
        excuteAction?.Invoke((T)Convert.ChangeType(parameter, typeof(T)));
}