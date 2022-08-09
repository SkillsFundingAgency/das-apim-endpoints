using MediatR;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Models;

namespace SFA.DAS.TrackProgress.Application.Commands
{
    public class TrackProgressCommand : IRequest<TrackProgressResponse>
    {
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public ProgressDto? Progress { get; set; } = null;

        public TrackProgressCommand(long ukprn, long uln, DateTime plannedStartDate, ProgressDto progressDto)
            => (Ukprn, Uln, PlannedStartDate, Progress) = (ukprn, uln, plannedStartDate, progressDto);
    }
}
