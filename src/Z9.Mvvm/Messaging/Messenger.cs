using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading;

namespace Z9.Mvvm.Messaging
{
	/// <summary>
	/// A tool for broadcasting message
	/// </summary>
	public sealed class Messenger
	{
		static Messenger _default;
		/// <summary>
		/// Messenger instance
		/// </summary>
		public static Messenger Default
		{
			get
			{
				if(_default == null)
				{
					var ins = new Messenger();
					Interlocked.CompareExchange(ref _default, ins, null);
				}
				return _default;
			}
		}

		Dictionary<Type, List<Tuple<Delegate, object>>> actList = new Dictionary<Type, List<Tuple<Delegate, object>>>();

		/// <summary>
		/// Send message
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="msg">Message</param>
		public void Send<T>(T msg)
		{
			var acts = from act in actList
					   from del in act.Value
					   where del.Item2 == null && del.Item1.GetMethodInfo().GetParameters()[0].ParameterType == typeof(T)
					   select del.Item1;
			foreach (var act in acts)
				act?.DynamicInvoke(msg);
		}

		/// <summary>
		/// Send message
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="msg">Message</param>
		/// <param name="token">Message token</param>
		public void Send<T>(T msg, object token)
		{
			if (token == null)
				throw new InvalidOperationException("Message token must not be null");

			var acts = from act in actList
					   from del in act.Value
					   where del.Item2.Equals(token) && del.Item1.GetMethodInfo().GetParameters()[0].ParameterType == typeof(T)
					   select del.Item1;
			foreach (var act in acts)
				act?.DynamicInvoke(msg);
		}

		/// <summary>
		/// Register the message
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="reciepient">Message receiver</param>
		/// <param name="opt">Delegate</param>
		public void Register<T>(object reciepient, Action<T> opt) => Register(reciepient, opt, null);

		/// <summary>
		/// Register the message with token
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="reciepient">Message receiver</param>
		/// <param name="token">Message token</param>
		/// <param name="opt">Delegate</param>
		public void Register<T>(object reciepient, object token, Action<T> opt)
		{
			if (token == null)
				throw new InvalidOperationException("Message token must not be null");
			Register(reciepient, opt, token);
		}

		/// <summary>
		/// Unregister all message for target receiver
		/// </summary>
		/// <param name="reciepient">receiver</param>
		public void Unregister(object reciepient)
		{
			var t = reciepient.GetType();
			actList.Remove(t);
		}

		void Register<T>(object reciepient, Action<T> opt, object token)
		{
			var t = reciepient.GetType();
			if (!actList.ContainsKey(t))
				actList.Add(t, new List<Tuple<Delegate, object>>());

			actList[t].Add(Tuple.Create<Delegate, object>(opt, token));
		}

		Messenger() { }
	}
}
