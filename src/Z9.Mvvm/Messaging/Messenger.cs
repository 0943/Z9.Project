using System;
using System.Collections.Generic;
using System.Reflection;
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
				if (_default == null)
				{
					var ins = new Messenger();
					Interlocked.CompareExchange(ref _default, ins, null);
				}
				return _default;
			}
		}

		Dictionary<Type, List<Tuple<Delegate, Type, object>>> actList = new Dictionary<Type, List<Tuple<Delegate, Type, object>>>();

		/// <summary>
		/// Send message
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="msg">Message</param>
		/// <exception cref="MemberAccessException"/>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="TargetInvocationException"/>
		public void Send<T>(T msg)
		{
			if (msg == null)
				throw new ArgumentNullException("Message must not be null");

			foreach (var act in actList)
				foreach (var del in act.Value)
					if (del.Item3 == null && del.Item2.Equals(typeof(T)))
						try { del.Item1?.DynamicInvoke(msg); }
						catch { throw; }
		}

		/// <summary>
		/// Send message to target type. Indicate target type can get better performance.
		/// </summary>
		/// <typeparam name="TMessage">Message type</typeparam>
		/// <typeparam name="TTarget">Target type</typeparam>
		/// <param name="msg">Message</param>
		public void Send<TMessage, TTarget>(TMessage msg) where TTarget : class
		{
			if (msg == null)
				throw new ArgumentNullException("Message must not be null");

			foreach (var recipient in actList)
			{
				if (recipient.Key.Equals(typeof(TTarget)))
				{
					foreach (var del in recipient.Value)
						if (del.Item3 == null && del.Item2.Equals(typeof(TMessage)))
							try { del.Item1?.DynamicInvoke(msg); }
							catch { throw; }
					break;
				}
			}
		}

		/// <summary>
		/// Send message with token
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="msg">Message</param>
		/// <param name="token">Message token</param>
		/// <exception cref="MemberAccessException"/>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="TargetInvocationException"/>
		public void Send<T>(T msg, object token)
		{
			if (token == null)
				throw new ArgumentNullException("Message token must not be null");
			if (msg == null)
				throw new ArgumentNullException("Message must not be null");

			foreach (var act in actList)
				foreach (var del in act.Value)
					if (Equals(del.Item3, token) && del.Item2.Equals(typeof(T)))
						try { del.Item1?.DynamicInvoke(msg); }
						catch { throw; }
		}

		/// <summary>
		/// Send message to target type with token. Indicate target type can get better performance.
		/// </summary>
		/// <typeparam name="TMessage">Message type</typeparam>
		/// <typeparam name="TTarget">Target type</typeparam>
		/// <param name="msg">Message</param>
		/// <param name="token">Messaget token</param>
		public void Send<TMessage, TTarget>(TMessage msg, object token)
		{
			if (token == null)
				throw new ArgumentNullException("Message token must not be null");
			if (msg == null)
				throw new ArgumentNullException("Message must not be null");

			foreach (var recipient in actList)
			{
				if (recipient.Key.Equals(typeof(TTarget)))
				{
					foreach (var del in recipient.Value)
						if (Equals(del.Item3, token) && del.Item2.Equals(typeof(TMessage)))
							try { del.Item1?.DynamicInvoke(msg); }
							catch { throw; }
					break;
				}
			}
		}

		/// <summary>
		/// Register the message
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="reciepient">Message receiver</param>
		/// <param name="opt">Delegate</param>
		/// <exception cref="ArgumentNullException">Reciepient is null</exception>
		public void Register<T>(object reciepient, Action<T> opt)
		{
			try { Register(reciepient, opt, null); }
			catch { throw; }
		}

		/// <summary>
		/// Register the message with token
		/// </summary>
		/// <typeparam name="T">Message type</typeparam>
		/// <param name="reciepient">Message receiver</param>
		/// <param name="token">Message token</param>
		/// <param name="opt">Delegate</param>
		/// <exception cref="ArgumentNullException">Reciepient is null</exception>
		/// <exception cref="ArgumentNullException">Token is null</exception>
		public void Register<T>(object reciepient, object token, Action<T> opt)
		{
			if (token == null)
				throw new ArgumentNullException("Message token must not be null");
			try { Register(reciepient, opt, token); }
			catch { throw; }
		}

		/// <summary>
		/// Unregister all message for target receiver
		/// </summary>
		/// <param name="reciepient">receiver</param>
		/// <exception cref="ArgumentNullException">Reciepient is null</exception>
		public void Unregister(object reciepient)
		{
			try
			{
				var t = reciepient.GetType();
				actList.Remove(t);
			}
			catch { throw; }
		}

		void Register<T>(object reciepient, Action<T> opt, object token)
		{
			if (reciepient == null)
				throw new ArgumentNullException("Reciepient must not be null");

			var t = reciepient.GetType();
			if (!actList.ContainsKey(t))
				actList.Add(t, new List<Tuple<Delegate, Type, object>>());

			actList[t].Add(Tuple.Create<Delegate, Type, object>(opt, typeof(T), token));
		}

		Messenger() { }
	}
}