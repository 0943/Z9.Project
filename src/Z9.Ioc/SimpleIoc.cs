using System;
using System.Collections.Generic;
using System.Threading;

namespace Z9.Ioc
{
	/// <summary>
	/// Simple dynamic singleton helper, instances in this object is dynamic singleton and will be finalized when there's no reference
	/// (not compeletly singleton, you can use key to break it)
	/// </summary>
	public sealed class SimpleIoc
	{
		static SimpleIoc _default;
		/// <summary>
		/// Ioc Instance
		/// </summary>
		public static SimpleIoc Default
		{
			get
			{
				if (_default == null)
				{
					var ins = new SimpleIoc();
					Interlocked.CompareExchange(ref _default, ins, null);
				}
				return _default;
			}
		}

		Dictionary<Type, WeakReference> singletonList = new Dictionary<Type, WeakReference>();

		/// <summary>
		/// Register type
		/// </summary>
		/// <typeparam name="TInstance">class type</typeparam>
		public void Register<TInstance>() where TInstance : class
		{
			Register(typeof(TInstance));
		}

		/// <summary>
		/// Register type
		/// </summary>
		/// <param name="type">class type</param>
		public void Register(Type type)
		{
			try
			{
				singletonList.Add(type, new WeakReference(null));
			}
			catch { }
		}

		/// <summary>
		/// Get instance with target type
		/// </summary>
		/// <typeparam name="TInstance">instance type</typeparam>
		/// <returns>instance</returns>
		/// <exception cref="InvalidOperationException">Target type not be registered before</exception>
		/// <exception cref="MissingMethodException">Target type don't have none parameter constructor</exception>
		public TInstance GetInstance<TInstance>() where TInstance : class
		{
			try
			{
				return (TInstance)GetInstance(typeof(TInstance));
			}
			catch { throw; }
		}

		/// <summary>
		/// Get instance with target type
		/// </summary>
		/// <param name="type">instance type</param>
		/// <returns>instance</returns>
		/// <exception cref="InvalidOperationException">Target type not be registered before</exception>
		/// <exception cref="MissingMethodException">Target type don't have none parameter constructor</exception>
		public object GetInstance(Type type)
		{
			WeakReference weakR;
			try
			{
				weakR = singletonList[type];
			}
			catch
			{
				throw new InvalidOperationException($"Type \"{type.FullName}\" not exist");
			}

			if (weakR.IsAlive)
				return weakR.Target;

			object ins = default;
			try
			{
				ins = Activator.CreateInstance(type);
			}
			catch { throw; }

			weakR.Target = ins;
			return ins;
		}

		SimpleIoc() { }
	}
}