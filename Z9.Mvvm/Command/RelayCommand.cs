using System;

namespace Z9.Mvvm.Command;

/// <summary>
/// Mvvm command tool
/// </summary>
public class RelayCommand : CommandBase
{
	Action excuteAction;
	Func<bool> canExcuteFunc;

	/// <summary>
	/// Create command instance
	/// </summary>
	/// <param name="excute">The execution logic</param>
	/// <param name="canExcute">The execution status logic</param>
	public RelayCommand(Action excute, Func<bool> canExcute = null)
	{
		excuteAction = excute;
		canExcuteFunc = canExcute;
	}

	/// <summary>
	/// Define a method that determines whether the command can execute
	/// </summary>
	/// <param name="parameter">This parameter will be always ignored</param>
	/// <returns>true</returns>
	public override bool CanExecute(object parameter) => 
		canExcuteFunc == null || canExcuteFunc();

	/// <summary>
	/// Define a method to be called when command is invoked
	/// </summary>
	/// <param name="parameter">This parameter will be always triggered</param>
	public override void Execute(object parameter) => 
		excuteAction?.Invoke();
}
