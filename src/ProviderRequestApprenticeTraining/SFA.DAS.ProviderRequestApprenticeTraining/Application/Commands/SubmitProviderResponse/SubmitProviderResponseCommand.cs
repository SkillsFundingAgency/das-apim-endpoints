using System.Collections.Generic;
using System;
using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse
{
    public class SubmitProviderResponseCommand : IRequest<SubmitProviderResponseResponse>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; } = new List<Guid>();
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string CurrentUserEmail { get; set; }
    }
}
