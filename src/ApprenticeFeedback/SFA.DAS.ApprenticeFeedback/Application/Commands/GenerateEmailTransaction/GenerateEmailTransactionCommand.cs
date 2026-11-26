using MediatR;
using SFA.DAS.Apim.Shared.Infrastructure;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction
{
    public class GenerateEmailTransactionCommand : IRequest<NullResponse>
    {
    }
}
