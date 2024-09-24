using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.PostStandard
{
    public class PostStandardCommand : IRequest<PostStandardResult>
    {
        public string StandardId { get; set; }
    }
}
