using MediatR;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses
{
    public class GetProviderEmailAddressesQuery : IRequest<GetProviderEmailAddressesResult>
    {
        public long Ukprn { get; set; }
        public string UserEmailAddress { get; set; }
    }
}
