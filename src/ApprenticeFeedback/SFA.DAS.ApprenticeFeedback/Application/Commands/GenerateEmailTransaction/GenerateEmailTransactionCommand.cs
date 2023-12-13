using MediatR;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction
{
    public class GenerateEmailTransactionCommand : IRequest<NullResponse>
    {
    }
}
