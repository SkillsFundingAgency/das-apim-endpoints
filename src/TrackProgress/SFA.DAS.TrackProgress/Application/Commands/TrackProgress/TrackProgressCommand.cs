using MediatR;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Models;

namespace SFA.DAS.TrackProgress.Application.Commands.TrackProgress;

public record TrackProgressCommand(
    ProviderContext ProviderContext,
    long Uln,
    DateTime PlannedStartDate,
    ProgressDto? Progress) : IRequest<TrackProgressResponse>;
