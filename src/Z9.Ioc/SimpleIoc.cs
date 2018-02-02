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
		Dictionary<string, Tuple<Type, WeakReference>> kInstanceList = new Dictionary<string, Tuple<Type, WeakReference>>();

		/// <summary>
		/// Register type
		/// </summary>
		/// <typeparam name="TInstance">class type</typeparam>
		public void Register<TInstance>() where TInstance : class
		{
			try
			{
				singletonList.Add(typeof(TInstance), new WeakReference(null));
			}
			catch { }
		}

		/// <summary>
		/// Register type with identifier
		/// </summary>
		/// <typeparam name="TInstance">class type</typeparam>
		/// <param name="key">identifier</param>
		/// <exception cref="ArgumentNullException">Key is null</exception>
		public void Register<TInstance>(string key) where TInstance : class
		{
			try
			{
				kInstanceList.Add(key, Tuple.Create(typeof(TInstance), new WeakReference(null)));
			}
			catch (ArgumentNullException)
			{
				throw new ArgumentNullException("Key must not be null");
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
		/// Get instance with target type and key
		/// </summary>
		/// <typeparam name="TInstance">instance type</typeparam>
		/// <param name="key">identifier</param>
		/// <returns>instance</returns>
		/// <exception cref="ArgumentException">Target key not be registered before or key is null</exception>
		/// <exception cref="InvalidOperationException">Target type isn't the same with registered type</exception>
		/// <exception cref="MissingMethodException">Target type don't have none parameter constructor</exception>
		public TInstance GetInstance<TInstance>(string key) where TInstance : class
		{
			Tuple<Type, WeakReference> kInstance;
			try
			{
				kInstance = kInstanceList[key];
			}
			catch
			{
				throw new ArgumentException($"Key is null or there's no \"{key}\" yet");
			}

			var weakR = kInstance.Item2;
			try
			{
				if (weakR.IsAlive)
					return (TInstance)weakR.Target;
			}
			catch
			{
				throw new InvalidOperationException("Target type isn't the same with registered type");
			}

			TInstance ins = default;
			try
			{
				ins = Activator.CreateInstance<TInstance>();
			}
			catch (MissingMethodException ex)
			{
				throw new MissingMethodException($"Create \"{typeof(TInstance).FullName}\" fail (\"{key}\")", ex);
			}

			weakR.Target = ins;
			return ins;
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