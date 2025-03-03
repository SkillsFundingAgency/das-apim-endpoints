using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard
{
    public class GetStandardQuery : IRequest<GetStandardResult>
    {
        public string StandardReference { get; set; }
    }
}
