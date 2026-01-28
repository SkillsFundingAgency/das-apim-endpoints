using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.AccessorService;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.AccessorService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProvider;

public sealed class WhenGettingCourseProvider
{
    private Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> _roatpClientMock;
    private Mock<IAssessorsApiClient<AssessorsApiConfiguration>> _assessorClientMock;
    private Mock<ICachedLocationLookupService> _cachedLocationLookupServiceMock;
    private Mock<ICachedFeedbackService> _cachedFeedbackServiceMock;

    private GetCourseProviderQueryHandler SetupHandler(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetAssessmentsResponse assessmentResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        _roatpClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        _assessorClientMock = new Mock<IAssessorsApiClient<AssessorsApiConfiguration>>();
        _cachedLocationLookupServiceMock = new Mock<ICachedLocationLookupService>();
        _cachedFeedbackServiceMock = new Mock<ICachedFeedbackService>();

        _cachedLocationLookupServiceMock
            .Setup(x => x.GetCachedLocationInformation(query.Location, false))
            .ReturnsAsync(locationResponse);

        if (courseProviderDetailsResponse == null)
        {
            _roatpClientMock
                .Setup(x => x.GetWithResponseCode<GetCourseProviderDetailsResponse>(It.IsAny<GetCourseProviderDetailsRequest>()))
                .ReturnsAsync(new ApiResponse<GetCourseProviderDetailsResponse>(null, HttpStatusCode.GatewayTimeout, string.Empty));
        }
        else
        {
            _roatpClientMock
                .Setup(x => x.GetWithResponseCode<GetCourseProviderDetailsResponse>(
                    It.Is<GetCourseProviderDetailsRequest>(a =>
                        a.GetUrl.Contains($"location={query.Location}") &&
                        a.GetUrl.Contains($"latitude={locationResponse.Latitude}") &&
                        a.GetUrl.Contains($"longitude={locationResponse.Longitude}") &&
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

            _cachedFeedbackServiceMock
                .Setup(x => x.GetProviderFeedback(query.Ukprn))
            .ReturnsAsync((employerFeedbackResponse, apprenticeFeedbackResponse));

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
                        a.LarsCodes.SequenceEqual(new string[] { query.LarsCode }) &&
                        a.Distance.Equals(query.Distance) &&
                        a.Latitude.Equals(locationResponse.Latitude) &&
                        a.Longitude.Equals(locationResponse.Longitude)
                    )
                ))
            .ReturnsAsync(new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                courseTrainingProvidersCountResponse,
                HttpStatusCode.OK,
                string.Empty
            ));
        }

        return new GetCourseProviderQueryHandler(
            _roatpClientMock.Object,
            _assessorClientMock.Object,
            _cachedLocationLookupServiceMock.Object,
            _cachedFeedbackServiceMock.Object
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Cached_Location_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
            query,
            courseProviderDetailsResponse,
            assessmentResponse,
            employerFeedbackResponse,
            apprenticeFeedbackResponse,
            providerCoursesResponse,
            courseTrainingProvidersCountResponse,
            locationResponse
        );

        await sut.Handle(query, CancellationToken.None);

        _cachedLocationLookupServiceMock.Verify(
            x => x.GetCachedLocationInformation(query.Location, false),
            Times.Once
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Roatp_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse

    )
    {
        var sut = SetupHandler(
            query,
            courseProviderDetailsResponse,
            assessmentResponse,
            employerFeedbackResponse,
            apprenticeFeedbackResponse,
            providerCoursesResponse,
            courseTrainingProvidersCountResponse,
            locationResponse
        );

        var actualResult = await sut.Handle(query, CancellationToken.None);

        _roatpClientMock.Verify(x =>
            x.GetWithResponseCode<GetCourseProviderDetailsResponse>(It.Is<GetCourseProviderDetailsRequest>(a =>
                a.GetUrl.Contains($"location={query.Location}") &&
                a.GetUrl.Contains($"latitude={locationResponse.Latitude}") &&
                a.GetUrl.Contains($"longitude={locationResponse.Longitude}") &&
                a.GetUrl.Contains($"shortlistUserId={query.ShortlistUserId}")
            )),
            Times.Once
        );
        actualResult.Should().NotBeNull();
    }

    [Test, AutoData]
    public async Task Then_Raise_Exception_If_Roatp_Api_Returns_Failed_Status_Code(
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        GetCourseProviderQuery query,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse

    )
    {
        var sut = SetupHandler(
            query,
            null,
            assessmentResponse,
            employerFeedbackResponse,
            apprenticeFeedbackResponse,
            providerCoursesResponse,
            courseTrainingProvidersCountResponse,
            locationResponse
        );

        Func<Task> action = () => sut.Handle(query, CancellationToken.None);

        await action.Should().ThrowAsync<ApiResponseException>();

        _roatpClientMock.Verify(x =>
            x.GetWithResponseCode<GetCourseProviderDetailsResponse>(It.Is<GetCourseProviderDetailsRequest>(a =>
                a.GetUrl.Contains($"location={query.Location}") &&
                a.GetUrl.Contains($"latitude={locationResponse.Latitude}") &&
                a.GetUrl.Contains($"longitude={locationResponse.Longitude}") &&
                a.GetUrl.Contains($"shortlistUserId={query.ShortlistUserId}")
            )),
            Times.Once
        );
        _assessorClientMock.Verify(x => x.GetWithResponseCode<GetAssessmentsResponse>(It.IsAny<GetAssessmentsRequest>()), Times.Never);
        _cachedFeedbackServiceMock.Verify(x => x.GetProviderFeedback(query.Ukprn), Times.Never);
        _roatpClientMock.Verify(c => c.GetWithResponseCode<List<ProviderCourseResponse>>(It.IsAny<ProviderCoursesRequest>()), Times.Never);
        _roatpClientMock.Verify(x => x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(It.IsAny<GetCourseTrainingProvidersCountRequest>()), Times.Never);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Accessor_Api_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             assessmentResponse,
             employerFeedbackResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        var actualResult = await sut.Handle(query, CancellationToken.None);

        _assessorClientMock.Verify(x =>
            x.GetWithResponseCode<GetAssessmentsResponse>(It.Is<GetAssessmentsRequest>(a =>
                a.Ukprn.Equals(query.Ukprn) &&
                a.IFateReferenceNumber.Equals(courseProviderDetailsResponse.IFateReferenceNumber)
            )),
            Times.Once
        );
        actualResult.EndpointAssessments.Should().BeEquivalentTo(assessmentResponse);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Feedback_Service_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             assessmentResponse,
             employerFeedbackResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        var actualResult = await sut.Handle(query, CancellationToken.None);

        _cachedFeedbackServiceMock.Verify(x => x.GetProviderFeedback(query.Ukprn), Times.Once);
        actualResult.AnnualEmployerFeedbackDetails.Should().BeEquivalentTo(employerFeedbackResponse.AnnualEmployerFeedbackDetails);
        actualResult.AnnualApprenticeFeedbackDetails.Should().BeEquivalentTo(apprenticeFeedbackResponse.AnnualApprenticeFeedbackDetails);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Roatp_Api_For_Course_Details_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             assessmentResponse,
             employerFeedbackResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        var actualResult = await sut.Handle(query, CancellationToken.None);

        _roatpClientMock.Verify(x =>
            x.GetWithResponseCode<List<ProviderCourseResponse>>(
                It.Is<ProviderCoursesRequest>(a =>
                    a.GetUrl.Contains(query.Ukprn.ToString())
                )
            ),
            Times.Once
        );
        actualResult.Courses.Should().BeEquivalentTo(providerCoursesResponse);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_Handle_Calls_The_Roatp_Api_For_Provider_Count_With_The_Correct_Parameters(
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse
    )
    {
        var sut = SetupHandler(
             query,
             courseProviderDetailsResponse,
             assessmentResponse,
             employerFeedbackResponse,
             apprenticeFeedbackResponse,
             providerCoursesResponse,
             courseTrainingProvidersCountResponse,
             locationResponse
        );

        var actualResult = await sut.Handle(query, CancellationToken.None);

        _roatpClientMock.Verify(x =>
            x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                    a.LarsCodes.SequenceEqual(new string[] { query.LarsCode }) &&
                    a.Distance.Equals(query.Distance) &&
                    a.Latitude.Equals(locationResponse.Latitude) &&
                    a.Longitude.Equals(locationResponse.Longitude)
                )
            ),
            Times.Once
        );
        actualResult.TotalProvidersCount.Should().Be(courseTrainingProvidersCountResponse.Courses[0].TotalProvidersCount);
    }

    [Test]
    [MoqInlineAutoData(HttpStatusCode.NotFound)]
    [MoqInlineAutoData(HttpStatusCode.BadRequest)]
    public async Task And_Roatp_Api_Call_Returns_404_Or_400_Handler_Returns_Null(
        HttpStatusCode statusCode,
        GetCourseProviderQuery query,
        GetCourseProviderDetailsResponse courseProviderDetailsResponse,
        EmployerFeedbackAnnualDetails employerFeedbackResponse,
        GetAssessmentsResponse assessmentResponse,
        ApprenticeFeedbackAnnualDetails apprenticeFeedbackResponse,
        List<ProviderCourseResponse> providerCoursesResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse,
        LocationItem locationResponse)
    {
        var sut = SetupHandler(
            query,
            courseProviderDetailsResponse,
            assessmentResponse,
            employerFeedbackResponse,
            apprenticeFeedbackResponse,
            providerCoursesResponse,
            courseTrainingProvidersCountResponse,
            locationResponse
        );

        _roatpClientMock.Setup(x => x.GetWithResponseCode<GetCourseProviderDetailsResponse>(
            It.IsAny<GetCourseProviderDetailsRequest>())).ReturnsAsync(new ApiResponse<GetCourseProviderDetailsResponse>(
            null,
            statusCode,
            string.Empty
        ));

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().Be(null);
        _assessorClientMock.Verify(x => x.GetWithResponseCode<GetAssessmentsResponse>(It.IsAny<GetAssessmentsRequest>()), Times.Never);
        _cachedFeedbackServiceMock.Verify(x => x.GetProviderFeedback(query.Ukprn), Times.Never);
        _roatpClientMock.Verify(c => c.GetWithResponseCode<List<ProviderCourseResponse>>(It.IsAny<ProviderCoursesRequest>()), Times.Never);
        _roatpClientMock.Verify(x => x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(It.IsAny<GetCourseTrainingProvidersCountRequest>()), Times.Never);
    }
}
