using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.AccessorService;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.AccessorService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProvider;

public sealed class WhenGettingCourseProvider
{
    private readonly Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> _roatpClientMock = new();
    private readonly Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _employerFeedbackMock = new();
    private readonly Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _apprenticeFeedbackMock = new();
    private readonly Mock<IAssessorsApiClient<AssessorsApiConfiguration>> _assessorClientMock = new();
    private readonly Mock<ICachedLocationLookupService> _cachedLocationLookupService = new();

    private GetCourseProviderQueryHandler SetupHandler(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationReponse
    )
    {
        _cachedLocationLookupService
            .Setup(x => x.GetCachedLocationInformation(query.Location, false))
            .ReturnsAsync(locationReponse);

        _roatpClientMock
            .Setup(x => x.GetWithResponseCode<GetCourseProviderDetailsResponse>(
                It.Is<GetCourseProviderDetailsRequest>(a =>
                    a.GetUrl.Contains($"location={query.Location}") &&
                    a.GetUrl.Contains($"latitude={locationReponse.Latitude}") &&
                    a.GetUrl.Contains($"longitude={locationReponse.Longitude}") &&
                    a.GetUrl.Contains($"shortlistUserId={query.ShortlistUserId}")
                )
            ))
        .ReturnsAsync(new ApiResponse<GetCourseProviderDetailsResponse>(
            courseProviderDetailsResponse,
            HttpStatusCode.OK,
            string.Empty
        ));

        _assessorClientMock
            .Setup(x => x.GetWithResponseCode<GetAssessmentsResponse>(
                It.Is<GetAssessmentsRequest>(a =>
                    a.Ukprn.Equals(query.Ukprn) &&
                    a.IFateReferenceNumber.Equals(courseProviderDetailsResponse.IFateReferenceNumber)
                )
            ))
        .ReturnsAsync(new ApiResponse<GetAssessmentsResponse>(
            assessmentResponse,
            HttpStatusCode.OK,
            string.Empty
        ));

        _employerFeedbackMock
            .Setup(x => x.GetWithResponseCode<GetEmployerFeedbackSummaryAnnualResponse>(
                It.Is<GetEmployerFeedbackSummaryAnnualRequest>(a =>
                    a.GetUrl.Contains(query.Ukprn.ToString())
                )
            ))
        .ReturnsAsync(new ApiResponse<GetEmployerFeedbackSummaryAnnualResponse>(
            employerFeedbackResponse,
            HttpStatusCode.OK,
            string.Empty
        ));

        _apprenticeFeedbackMock
            .Setup(x => x.GetWithResponseCode<GetApprenticeFeedbackSummaryAnnualResponse>(
                It.Is<GetApprenticeFeedbackSummaryAnnualRequest>(a =>
                    a.GetUrl.Contains(query.Ukprn.ToString())
                )
            ))
        .ReturnsAsync(new ApiResponse<GetApprenticeFeedbackSummaryAnnualResponse>(
            apprenticeFeedbackResponse,
            HttpStatusCode.OK,
            string.Empty
        ));

        _roatpClientMock
            .Setup(x => x.GetWithResponseCode<List<ProviderCourseResponse>>(
                It.Is<ProviderCoursesRequest>(a =>
                    a.GetUrl.Contains(query.Ukprn.ToString())
                )
            ))
        .ReturnsAsync(new ApiResponse<List<ProviderCourseResponse>>(
            providerCoursesResponse,
            HttpStatusCode.OK,
            string.Empty
        ));

        _roatpClientMock
            .Setup(x => x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                    a.LarsCodes.SequenceEqual(new int[] { query.LarsCode }) &&
                    a.Distance.Equals(query.Distance) &&
                    a.Latitude.Equals(locationReponse.Latitude) &&
                    a.Longitude.Equals(locationReponse.Longitude)
                )
            ))
        .ReturnsAsync(new ApiResponse<GetCourseTrainingProvidersCountResponse>(
            courseTrainingProvidersCountResponse,
            HttpStatusCode.OK,
            string.Empty
        ));

        return new GetCourseProviderQueryHandler(
            _roatpClientMock.Object,
            _employerFeedbackMock.Object,
            _apprenticeFeedbackMock.Object,
            _assessorClientMock.Object,
            _cachedLocationLookupService.Object
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Cached_Location_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
            query, 
            courseProviderDetailsResponse, 
            employerFeedbackResponse, 
            assessmentResponse, 
            apprenticeFeedbackResponse, 
            providerCoursesResponse, 
            courseTrainingProvidersCountResponse, 
            locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _cachedLocationLookupService.Verify(
            x => x.GetCachedLocationInformation(query.Location, false),
            Times.Once
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Roatp_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
        
    )
    {
        var sut = SetupHandler(
            query,
            courseProviderDetailsResponse,
            employerFeedbackResponse,
            assessmentResponse,
            apprenticeFeedbackResponse,
            providerCoursesResponse,
            courseTrainingProvidersCountResponse,
            locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _roatpClientMock.Verify(x =>
            x.GetWithResponseCode<GetCourseProviderDetailsResponse>(It.Is<GetCourseProviderDetailsRequest>(a =>
                a.GetUrl.Contains($"location={query.Location}") &&
                a.GetUrl.Contains($"latitude={locationResponse.Latitude}") &&
                a.GetUrl.Contains($"longitude={locationResponse.Longitude}") &&
                a.GetUrl.Contains($"shortlistUserId={query.ShortlistUserId}")
            )),
            Times.Once
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Accessor_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             employerFeedbackResponse,
             assessmentResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _assessorClientMock.Verify(x =>
            x.GetWithResponseCode<GetAssessmentsResponse>(It.Is<GetAssessmentsRequest>(a =>
                a.Ukprn.Equals(query.Ukprn) &&
                a.IFateReferenceNumber.Equals(courseProviderDetailsResponse.IFateReferenceNumber)
            )),
            Times.Once
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Employer_Feedback_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             employerFeedbackResponse,
             assessmentResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _employerFeedbackMock.Verify(x =>
            x.GetWithResponseCode<GetEmployerFeedbackSummaryAnnualResponse>(
                It.Is<GetEmployerFeedbackSummaryAnnualRequest>(a =>
                    a.GetUrl.Contains(query.Ukprn.ToString()
                )
            )),
            Times.Once
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Apprentice_Feedback_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             employerFeedbackResponse,
             assessmentResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _apprenticeFeedbackMock.Verify(x =>
            x.GetWithResponseCode<GetApprenticeFeedbackSummaryAnnualResponse>(
                It.Is<GetApprenticeFeedbackSummaryAnnualRequest>(a =>
                    a.GetUrl.Contains(query.Ukprn.ToString()
                )
            )),
            Times.Once
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Roatp_Api_For_Course_Details_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             employerFeedbackResponse,
             assessmentResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _roatpClientMock.Verify(x =>
            x.GetWithResponseCode<List<ProviderCourseResponse>>(
                It.Is<ProviderCoursesRequest>(a =>
                    a.GetUrl.Contains(query.Ukprn.ToString())
                )
            ),
            Times.Once
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Roatp_Api_For_Provider_Count_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetEmployerFeedbackSummaryAnnualResponse employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        GetApprenticeFeedbackSummaryAnnualResponse apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             employerFeedbackResponse,
             assessmentResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _roatpClientMock.Verify(x =>
            x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                    a.LarsCodes.SequenceEqual(new int[] { query.LarsCode }) &&
                    a.Distance.Equals(query.Distance) &&
                    a.Latitude.Equals(locationResponse.Latitude) &&
                    a.Longitude.Equals(locationResponse.Longitude)
                )
            ),
            Times.Once
        );
    }
}
