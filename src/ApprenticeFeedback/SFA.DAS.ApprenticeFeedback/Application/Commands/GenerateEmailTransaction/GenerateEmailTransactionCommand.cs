using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction
{
    public class GenerateEmailTransactionCommand : IRequest<GenerateEmailTransactionResponse>
    {
    }
}
