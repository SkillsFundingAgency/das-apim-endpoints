using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.Extensions;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.Application.Handlers;

public class AddLearnerDataCommandHandler(IInternalApiClient<LearnerDataInnerApiConfiguration> client, IInternalApiClient<CoursesApiConfiguration> coursesClient, ILogger<AddLearnerDataCommandHandler> logger)
    : IRequestHandler<AddLearnerDataCommand, bool>
{
    public async Task<bool> Handle(AddLearnerDataCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if(string.IsNullOrEmpty(command.LearnerData?.LarsCode))
            {
                throw new ArgumentNullException(nameof(command.LearnerData), "Learner data LarsCode cannot be null");
            }

            logger.LogInformation("Getting course details for course {0}", command.LearnerData.LarsCode);
            var course = await coursesClient.Get<StandardDetailResponse?>(new GetStandardDetailsByIdRequest(command.LearnerData.LarsCode));
            if (course == null)
            {
                throw new Exception($"No course found for LARS code {command.LearnerData.LarsCode}");
            }

            logger.LogInformation("Building PUT request to add new learner data");
            var innerRequest = CreateLearnerDataRequest(command.LearnerData, course);
            var request = new PutLearnerDataRequest(command.LearnerData.UKPRN, command.LearnerData.ULN, innerRequest);

            logger.LogInformation("Calling inner api to add new learner data");
            var response = await client.PutWithResponseCode<NullResponse>(request);
            if (!string.IsNullOrWhiteSpace(response.ErrorContent))
            {
                logger.LogInformation("Adding learner data returned status code {0} and error {1}", response.StatusCode, response.ErrorContent);
            }
            return (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred whilst constructing message or calling inner api to add learner data");
            throw;
        }
    }

    private LearnerDataRequest CreateLearnerDataRequest(LearnerDataIncomingRequest request, StandardDetailResponse course)
    {
        var learnerDataRequest = new LearnerDataRequest
        {
            ULN = request.ULN,
            UKPRN = request.UKPRN,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Email = request.Email,
            DoB = request.DoB,
            StartDate = request.StartDate,
            PlannedEndDate = request.PlannedEndDate,
            PercentageLearningToBeDelivered = request.PercentageLearningToBeDelivered,
            EpaoPrice = request.EpaoPrice,
            TrainingPrice = request.TrainingPrice,
            AgreementId = request.AgreementId,
            IsFlexiJob = request.IsFlexiJob,
            PlannedOTJTrainingHours = request.PlannedOTJTrainingHours,
            LarsCode = request.LarsCode,
            TrainingName = course.Title,
            ApprenticeshipType = course.ApprenticeshipType == null ? null : EnumExtensions.FromDescription<LearningType>(course.ApprenticeshipType),
            CorrelationId = request.CorrelationId,
            ReceivedDate = request.ReceivedDate,
            AcademicYear = request.AcademicYear,
            ConsumerReference = request.ConsumerReference
        };

        return learnerDataRequest;
    }
}