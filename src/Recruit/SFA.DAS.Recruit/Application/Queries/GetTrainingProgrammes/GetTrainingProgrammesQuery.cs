using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes
{
    public class GetTrainingProgrammesQuery : IRequest<GetTrainingProgrammesQueryResult>
    {
        public bool IncludeFoundationApprenticeships { get; set; }
    }
}