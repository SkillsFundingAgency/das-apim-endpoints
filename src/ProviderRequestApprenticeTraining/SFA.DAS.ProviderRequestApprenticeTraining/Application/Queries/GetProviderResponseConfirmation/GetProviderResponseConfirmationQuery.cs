using MediatR;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationQuery : IRequest<GetProviderResponseConfirmationResult>
    {
        public Guid ProviderResponseId { get; set; }
    }
}
