using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class CreateDraftShortCourseCommand : IRequest
{
    public long Ukprn { get; set; }
    public ShortCourseRequest ShortCourseRequest { get; set; }
}

public class CreateDraftShortCourseCommandHandler(
    ILogger<CreateDraftShortCourseCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    ICreateDraftShortCoursePostRequestBuilder createDraftShortCoursePostRequestBuilder,
    IMessageSession messageSession
) : IRequestHandler<CreateDraftShortCourseCommand>
{
    public async Task Handle(CreateDraftShortCourseCommand command, CancellationToken cancellationToken)
    {
        var requestData = createDraftShortCoursePostRequestBuilder.Build(command.ShortCourseRequest, command.Ukprn);

        await learningApiClient.PostWithResponseCode<CreateDraftShortCourseRequest, Guid>(new CreateDraftShortCourseApiPostRequest(requestData));

        await messageSession.Publish(MapToEvent(requestData));

        //todo failure checking and logging

    }

    private LearnerDataEvent MapToEvent(CreateDraftShortCourseRequest request)
    {
        return new LearnerDataEvent
        {
            //todo tech design says we should be able to use this event with the addition of a LearningType property but there are quite a few missing items not on the short course request
        };
    }
}