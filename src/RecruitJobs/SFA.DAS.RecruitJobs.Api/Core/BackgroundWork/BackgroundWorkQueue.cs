using System.Threading.Channels;

namespace SFA.DAS.RecruitJobs.Api.Core.BackgroundWork;

public delegate Task BackgroundWorkItem(IServiceProvider serviceProvider, CancellationToken cancellationToken);

public interface IBackgroundWorkQueue
{
    void Enqueue(string name, BackgroundWorkItem workItem);
    ValueTask<(string Name, BackgroundWorkItem WorkItem)> DequeueAsync(CancellationToken cancellationToken);
}

public class BackgroundWorkQueue : IBackgroundWorkQueue
{
    private readonly Channel<(string Name, BackgroundWorkItem WorkItem)> _channel =
        Channel.CreateUnbounded<(string, BackgroundWorkItem)>(new UnboundedChannelOptions { SingleReader = true });

    public void Enqueue(string name, BackgroundWorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        _channel.Writer.TryWrite((name, workItem));
    }

    public ValueTask<(string Name, BackgroundWorkItem WorkItem)> DequeueAsync(CancellationToken cancellationToken)
        => _channel.Reader.ReadAsync(cancellationToken);
}
