using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;
public record ProcessVacancyClosedEarlyCommand(long VacancyReference) : IRequest;