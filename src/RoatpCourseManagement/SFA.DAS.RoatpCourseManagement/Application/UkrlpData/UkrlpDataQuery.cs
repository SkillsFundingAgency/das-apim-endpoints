using MediatR;
using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Infrastructure.Ukrlp;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    public class UkrlpDataQuery : IRequest<GetUkrlpDataQueryResponse>
    {
       public List<long> Ukprns { get; set; }
       public DateTime? ProvidersUpdatedSince { get; set; }
    }
}
