using MediatR;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQuery : IRequest<GetSelectEmployerRequestsResult>
    {
        public string StandardReference { get; set; }
        public int Ukprn { get; set; }
    }
}
