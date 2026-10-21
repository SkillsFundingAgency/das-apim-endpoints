using MediatR;

namespace SFA.DAS.RoatpOversight.Application.Commands.AssignCourseTypesToProvider;

public record AssignCourseTypesToProviderCommand(int Ukprn, string UserId, string UserDisplayName) : IRequest;
