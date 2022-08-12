using MediatR;
using SFA.DAS.TrackProgress.Application.Models;

namespace SFA.DAS.TrackProgress.Application.Commands;

public static class TrackApprenticeProgress
{
	public record Command(
		  Ukprn Ukprn
		, long Uln
		, DateTime PlannedStartDate
		, Progress Progress
		) : IRequest;

	public record Progress
	{
	}

	public class CommandHandler : IRequestHandler<Command>
	{
		public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			return Unit.Task;
		}
	}
}