using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderResult>
    {
        public int Ukprn { get; set; }
    }
}
