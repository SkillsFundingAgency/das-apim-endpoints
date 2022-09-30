using MediatR;
using System;
using System.Collections.Generic;
using System.Net;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    public class UkrlpDataCommand : IRequest<HttpStatusCode>
    {
       public List<long> Ukprns { get; set; }
       public DateTime? ProvidersUpdatedSince { get; set; }
    }
}
