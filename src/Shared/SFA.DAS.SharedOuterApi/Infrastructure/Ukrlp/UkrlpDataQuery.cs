using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp
{
    public class UkrlpDataQuery : IRequest<GetUkrlpDataQueryResponse>
    {
        public List<long> Ukprns { get; set; }
        public DateTime? ProvidersUpdatedSince { get; set; }
    }
}
