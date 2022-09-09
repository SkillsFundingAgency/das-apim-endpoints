namespace SFA.DAS.TrackProgress.Application.Commands.TrackProgress;

public class TrackProgressResponse
{
    public TrackProgressResponse(long progressId)
    {
        ProgressId = progressId;
    }
    public long ProgressId { get; }
};
