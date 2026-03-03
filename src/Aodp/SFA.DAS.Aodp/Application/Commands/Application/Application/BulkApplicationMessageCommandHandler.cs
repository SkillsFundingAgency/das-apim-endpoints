using MediatR;
using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class BulkApplicationMessageCommandHandler
    : IRequestHandler<BulkApplicationMessageCommand, BaseMediatrResponse<BulkApplicationMessageCommandResponse>>
{
    private readonly IMediator _mediator;

    public BulkApplicationMessageCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<BaseMediatrResponse<BulkApplicationMessageCommandResponse>> Handle(
        BulkApplicationMessageCommand request,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<BulkApplicationMessageCommandResponse>
        {
            Value = new BulkApplicationMessageCommandResponse()
        };

        var errors = new List<BulkApplicationMessageErrorDto>();
        var updatedCount = 0;

        var messageType = request switch
        {
            { ShareWithSkillsEngland: true } => ApplicationMessageType.ApplicationSharedWithSkillsEngland,
            { ShareWithOfqual: true } => ApplicationMessageType.ApplicationSharedWithOfqual,
            { Unlock: true } => ApplicationMessageType.UnlockApplication,
            _ => throw new InvalidOperationException("No valid bulk action selected.")
        };

        foreach (var id in request.ApplicationIds)
        {
            var cmd = new CreateApplicationMessageCommand
            {
                ApplicationId = id,
                MessageType = messageType.ToString(),
                MessageText = string.Format("Bulk Action {0}", messageType.ToString()),
                SentByEmail = request.SentByEmail,
                SentByName = request.SentByName,
                UserType = request.UserType
            };

            var result = await _mediator.Send(cmd, cancellationToken);

            if (!result.Success)
            {
                errors.Add(new BulkApplicationMessageErrorDto
                {
                    ApplicationId = id,
                    ErrorType = BulkApplicationMessageErrorType.General
                });
                continue;
            }

            updatedCount++;
        }

        response.Value.RequestedCount = request.ApplicationIds.Count;
        response.Value.UpdatedCount = updatedCount;
        response.Value.ErrorCount = errors.Count;
        response.Value.Errors = errors;

        response.Success = true;
        return response;
    }
}
