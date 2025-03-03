using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class SubmitProviderResponse
    {
        public Guid ProviderResponseId { get; set; }

        public static implicit operator SubmitProviderResponse(SubmitProviderResponseResponse source)
        {
            return new SubmitProviderResponse { ProviderResponseId = source.ProviderResponseId };
        }
    }
}
