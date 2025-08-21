using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes
{
    public class GetTrainingProgrammesQuery : IRequest<GetTrainingProgrammesQueryResult>
    {
        public int? Ukprn {  get; set; }
    }
}