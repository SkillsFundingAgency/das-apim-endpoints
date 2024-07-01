using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard
{
    public class GetStandardQuery : IRequest<GetStandardResult>
    {
        public string StandardId { get; set; }
    }
}
