using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Ukrlp;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Ukrlp
{
    public class UkrlpDataQuery : IRequest<GetUkrlpDataQueryResponse>
    {
        public List<long> Ukprns { get; set; }
        public DateTime? ProvidersUpdatedSince { get; set; }
    }
}
