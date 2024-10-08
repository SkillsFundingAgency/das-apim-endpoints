using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CacheStandard
{
    public class CacheStandardCommand : IRequest<CacheStandardResult>
    {
        public string StandardLarsCode { get; set; }
    }
}
