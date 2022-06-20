using System;
using System.Windows.Input;

namespace Z9.Mvvm.Command;

/// <summary>
/// Mvvm command tool base class
/// </summary>
public abstract class CommandBase : ICommand
{
	/// <summary>
	/// Occurs when changes occur that affect whether the command should execute
	/// </summary>
	public event EventHandler CanExecuteChanged
	{
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}

	/// <summary>
	/// Define a method that determines whether the command can excute
	/// </summary>
	/// <param name="parameter">Para</param>
	/// <returns>Boolean</returns>
	public abstract bool CanExecute(object parameter);

	/// <summary>
	/// Define a method to be called when command is Triggered
	/// </summary>
	/// <param name="parameter">Para</param>
	public abstract void Execute(object parameter);

	/// <summary>
	/// Manually enumerate commands whether they can be triggered.
	/// </summary>
	public static void RaiseCanExecuteChanged() => 
		CommandManager.InvalidateRequerySuggested();
}
