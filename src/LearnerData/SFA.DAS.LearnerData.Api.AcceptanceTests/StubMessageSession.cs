using NServiceBus;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests;

internal class StubMessageSession : IMessageSession
{
    public readonly static List<object> SentMessages = new List<object>();

    public Task Send(object message, SendOptions options)
    {
        SentMessages.Add(message);
        return Task.CompletedTask;
    }
    public Task Send<T>(Action<T> messageConstructor, SendOptions options) => Task.CompletedTask;
    public Task Publish(object message, PublishOptions options) => Task.CompletedTask;
    public Task Publish<T>(Action<T> messageConstructor, PublishOptions options) => Task.CompletedTask;
    public Task Subscribe(Type eventType, SubscribeOptions options) => Task.CompletedTask;
    public Task Unsubscribe(Type eventType, UnsubscribeOptions options) => Task.CompletedTask;
}
