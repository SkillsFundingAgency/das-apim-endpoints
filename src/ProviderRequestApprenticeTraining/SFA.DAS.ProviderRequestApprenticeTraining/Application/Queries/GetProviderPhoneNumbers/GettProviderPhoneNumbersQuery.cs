using MediatR;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers
{
    public class GetProviderPhoneNumbersQuery : IRequest<GetProviderPhoneNumbersResult>
    {
        public long Ukprn { get; set; }
    }
}
