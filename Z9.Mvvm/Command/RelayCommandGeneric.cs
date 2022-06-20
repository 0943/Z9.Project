using System;

namespace Z9.Mvvm.Command;

/// <summary>
/// Mvvm command tool with parameter
/// </summary>
/// <typeparam name="T">CommandParameter Type</typeparam>
public class RelayCommand<T> : CommandBase
{
	Action<T> excuteAction;
	Func<T, bool> canExcuteFunc;

	/// <summary>
	/// Create command instance
	/// </summary>
	/// <param name="excute">The execution logic</param>
	/// <param name="canExcute">The execution status logic</param>
	public RelayCommand(Action<T> excute, Func<T, bool> canExcute = null)
	{
		excuteAction = excute;
		canExcuteFunc = canExcute;
	}

	/// <summary>
	/// Define a method that determines whether the command can execute
	/// </summary>
	/// <param name="parameter">Para</param>
	/// <returns>Whether executable</returns>
	public override bool CanExecute(object parameter) => 
		canExcuteFunc == null || canExcuteFunc((T)Convert.ChangeType(parameter,typeof(T)));

	/// <summary>
	/// Define a method to be called when command is triggered
	/// </summary>
	/// <param name="parameter">Para</param>
	public override void Execute(object parameter) => 
		excuteAction?.Invoke((T)Convert.ChangeType(parameter, typeof(T)));
}
