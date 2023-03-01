using System;

namespace Z9.Mvvm.Messaging;

/// <summary>
/// Inner defined message type with callback
/// </summary>
public class MessageWithCallBack
{
    Action action;

    /// <summary>
    /// Message taking with
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Create MessageWithCallBack instance
    /// </summary>
    /// <param name="act">Delegate</param>
    public MessageWithCallBack(Action act) =>
        action = act;

    /// <summary>
    /// Create MessageWithCallBack instance
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="act">Delegate</param>
    public MessageWithCallBack(string message, Action act)
    {
        Message = message;
        action = act;
    }

    /// <summary>
    /// Execute the delegate
    /// </summary>
    public void Execute() =>
        action?.Invoke();
}
