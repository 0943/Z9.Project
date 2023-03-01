using System;
using System.Collections.Generic;
using System.Threading;

namespace Z9.Mvvm.Messaging;

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

    Dictionary<Type, Dictionary<object, List<Tuple<Delegate, object>>>> msgPool = new();

    /// <summary>
    /// Register the message
    /// </summary>
    /// <typeparam name="TMsg">Message type</typeparam>
    /// <param name="reciepient">Message reciepient</param>
    /// <param name="action">Delegate method</param>
    /// <exception cref="ArgumentNullException">Reciepient is null</exception>
    public void Register<TMsg>(object reciepient, Action<TMsg> action)
    {
        try { Register(reciepient, null, action); }
        catch { throw; }
    }

    /// <summary>
    /// Register the message with token
    /// </summary>
    /// <typeparam name="TMsg">Message type</typeparam>
    /// <param name="reciepient">Message reciepient</param>
    /// <param name="token">Message token</param>
    /// <param name="action">Delegate method</param>
    /// <exception cref="ArgumentNullException">Reciepient is null</exception>
    public void Register<TMsg>(object reciepient, object token, Action<TMsg> action)
    {
        if (reciepient == null)
            throw new ArgumentNullException("Reciepient can't be null");

        var msgType = typeof(TMsg);
        if (!msgPool.ContainsKey(msgType))
            msgPool.Add(msgType, new Dictionary<object, List<Tuple<Delegate, object>>>());
        if (!msgPool[msgType].ContainsKey(reciepient))
            msgPool[msgType].Add(reciepient, new List<Tuple<Delegate, object>>());
        msgPool[msgType][reciepient].Add(Tuple.Create<Delegate, object>(action, token));
    }

    /// <summary>
    /// Send message
    /// </summary>
    /// <typeparam name="TMsg">Message type</typeparam>
    /// <param name="msg">Message</param>
    public void Send<TMsg>(TMsg msg) =>
        Send(msg, null);

    /// <summary>
    /// Send message with token
    /// </summary>
    /// <typeparam name="TMsg">Message type</typeparam>
    /// <param name="msg">Message</param>
    /// <param name="token">Message Token</param>
    public void Send<TMsg>(TMsg msg, object token)
    {
        if (msg == null)
            return;

        if (!msgPool.ContainsKey(typeof(TMsg)))
            return;
        Dictionary<object, List<Tuple<Delegate, object>>> targetMsg = msgPool[typeof(TMsg)];

        foreach (var reciepient in targetMsg)
            foreach (var del in reciepient.Value)
                if (Equals(del.Item2, token))
                    del.Item1?.DynamicInvoke(msg);
    }

    /// <summary>
    /// Unregister all message for target reciepient
    /// </summary>
    /// <param name="reciepient">reciepient</param>
    public void Unregister(object reciepient)
    {
        if (reciepient == null)
            return;
        foreach (var msgType in msgPool)
            if (msgType.Value.ContainsKey(reciepient))
                msgType.Value.Remove(reciepient);   // Confilict

        var delLst = new List<Type>();
        foreach (var msgType in msgPool)
            if (msgType.Value.Count == 0)
                delLst.Add(msgType.Key);
        foreach (var delItem in delLst)
            msgPool.Remove(delItem);
    }

    Messenger() { }
}
