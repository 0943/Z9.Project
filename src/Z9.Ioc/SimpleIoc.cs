using System;
using System.Collections.Generic;
using System.Threading;

namespace Z9.Ioc
{
	/// <summary>
	/// Simple dynamic singleton helper
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
		/// Register type. this method indicate instance is dynamic singleton and will be finalized when there's no reference
		/// </summary>
		/// <typeparam name="TInstance">Class type</typeparam>
		public void Register<TInstance>() where TInstance : class
		{
			var t = typeof(TInstance);
			if (!singletonList.ContainsKey(t))
				singletonList.Add(t, new WeakReference(null));
		}

		/// <summary>
		/// Get instance with target type (dynamic singleton using WeakReference)
		/// </summary>
		/// <typeparam name="TInstance">Instance type</typeparam>
		/// <returns>Instance</returns>
		/// <exception cref="InvalidOperationException"/>
		/// <exception cref="Exception"/>
		public TInstance GetInstance<TInstance>() where TInstance : class
		{
			var t = typeof(TInstance);
			if (!singletonList.ContainsKey(t))
				throw new InvalidOperationException($"Type {typeof(TInstance).Name} not exist");

			var weakR = singletonList[t];
			if (weakR.IsAlive)
				return (TInstance)weakR.Target;

			TInstance ins = default;
			try
			{
				ins = (TInstance)typeof(TInstance).Assembly.CreateInstance(typeof(TInstance).FullName);
			}
			catch (Exception ex)
			{
				throw new Exception("Create instance fail", ex);
			}

			weakR.Target = ins;
			return ins;
		}

		SimpleIoc() { }
	}
}
