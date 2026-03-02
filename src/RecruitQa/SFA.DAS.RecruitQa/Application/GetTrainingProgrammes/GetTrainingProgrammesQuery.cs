using MediatR;

namespace SFA.DAS.RecruitQa.Application.GetTrainingProgrammes;

public sealed record GetTrainingProgrammesQuery(int? Ukprn) : IRequest<GetTrainingProgrammesQueryResult>;