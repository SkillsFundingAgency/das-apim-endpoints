using NServiceBus;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests;

/// <summary>
/// No-op IMessageSession stub used in acceptance tests so that NServiceBus does not need to be
/// configured with a real connection string. The handlers that use IMessageSession only call
/// Publish, and those calls are currently commented out, so the stub is never actually invoked.
/// </summary>
internal class StubMessageSession : IMessageSession
{
    public readonly static List<object> PublishedMessages = new List<object>();

    public Task Send(object message, SendOptions options) => Task.CompletedTask;
    public Task Send<T>(Action<T> messageConstructor, SendOptions options) => Task.CompletedTask;
    public Task Publish(object message, PublishOptions options)
    {
        PublishedMessages.Add(message);
        return Task.CompletedTask;
    }

    public Task Publish<T>(Action<T> messageConstructor, PublishOptions options) => Task.CompletedTask;
    public Task Subscribe(Type eventType, SubscribeOptions options) => Task.CompletedTask;
    public Task Unsubscribe(Type eventType, UnsubscribeOptions options) => Task.CompletedTask;
}
