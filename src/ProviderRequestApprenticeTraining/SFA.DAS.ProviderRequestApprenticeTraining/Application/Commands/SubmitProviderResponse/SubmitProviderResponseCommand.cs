using System.Collections.Generic;
using System;
using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse
{
    public class SubmitProviderResponseCommand : IRequest<SubmitProviderResponseResponse>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; } = new List<Guid>();
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string CurrentUserEmail { get; set; }
        public Guid RespondedBy { get; set; }
        public string CurrentUserFirstName { get; set; }
        public string StandardLevel { get; set; }
        public string StandardTitle { get; set; }
    }
}
