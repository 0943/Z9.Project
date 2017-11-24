using System;

namespace Z9.Mvvm.Command
{
	/// <summary>
	/// Mvvm command tool
	/// </summary>
	public class RelayCommand : CommandBase
	{
		Action ExcuteAction { get; }
		Func<bool> CanExcuteFunc { get; }

		/// <summary>
		/// Create command instance
		/// </summary>
		/// <param name="excute">The execution logic</param>
		/// <param name="canExcute">The execution status logic</param>
		public RelayCommand(Action excute, Func<bool> canExcute = null)
		{
			ExcuteAction = excute;
			CanExcuteFunc = canExcute;
		}

		/// <summary>
		/// Define a method that determines whether the command can execute
		/// </summary>
		/// <param name="parameter">This parameter will be always ignored</param>
		/// <returns>true</returns>
		public override bool CanExecute(object parameter) => CanExcuteFunc == null ? true : CanExcuteFunc();

		/// <summary>
		/// Define a method to be called when command is invoked
		/// </summary>
		/// <param name="parameter">This parameter will be always triggered</param>
		public override void Execute(object parameter) => ExcuteAction();
	}
}
