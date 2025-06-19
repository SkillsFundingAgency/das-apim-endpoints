using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Services;

public class WhenAddingExtendedPropertiesToCsvRecords
{
    [Test, MoqAutoData]
    public async Task Then_It_Add_Returns_Each_Record_Mapped_ToExtendedObject(
        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        csvRecords.ForEach(x => x.CourseCode = null);
        mappedRecord.CourseCode = null;
        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var result = await sut.PopulateWithCourseTypeData(csvRecords);
        result.FirstOrDefault().Should().BeEquivalentTo(mappedRecord);
    }

    [Test, MoqAutoData]
    public async Task Then_It_Add_Age_Range_Limits_To_Each_Record(
        BulkUploadAddDraftApprenticeshipRequest csvRecord,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        string courseType,
        GetLearnerAgeResponse apiResponse,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApi,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        courseTypesApi.Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
            .ReturnsAsync(apiResponse);

        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var result = await sut.PopulateWithCourseTypeData(new List<BulkUploadAddDraftApprenticeshipRequest> { csvRecord });
        result.FirstOrDefault().Should().BeEquivalentTo(mappedRecord);
        result[0].MinimumAgeAtApprenticeshipStart.Should().Be(apiResponse.MinimumAge);
        result[0].MaximumAgeAtApprenticeshipStart.Should().Be(apiResponse.MaximumAge);
    }

    [Test, MoqAutoData]
    public async Task When_On_Same_Course_Then_It_Only_Calls_Api_Once(
        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        GetLearnerAgeResponse apiResponse,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApi,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        csvRecords.ForEach(x => x.CourseCode = "ABCDE");
        mappedRecord.CourseCode = null;

        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var result = await sut.PopulateWithCourseTypeData(csvRecords);
        courseTypesApi.Verify(x=>x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()), Times.Once);
        coursesApiClient.Verify(x=>x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_CourseType_Not_Found_Then_Throw_Exception(
        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        GetStandardsListItem course,
        GetLearnerAgeResponse apiResponse,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApi,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        csvRecords.ForEach(x => x.CourseCode = "ABCDE");
        mappedRecord.CourseCode = null;

        coursesApiClient.Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
            .ReturnsAsync(course);

        courseTypesApi.Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
            .ReturnsAsync((GetLearnerAgeResponse)null);

        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var act = async () => await sut.PopulateWithCourseTypeData(csvRecords);
        await act.Should().ThrowAsync<Exception>().WithMessage($"Learner Age response for {course.ApprenticeshipType} was not found");
        courseTypesApi.Verify(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()), Times.Once);
        coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_Course_Not_Found_Then_Throw_Exception(
        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        GetStandardsListItem course,
        GetLearnerAgeResponse apiResponse,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApi,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        csvRecords.ForEach(x => x.CourseCode = "ABCDE");
        mappedRecord.CourseCode = null;

        coursesApiClient.Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
            .ReturnsAsync((GetStandardsListItem)null);

        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var act = async () => await sut.PopulateWithCourseTypeData(csvRecords);
        await act.Should().ThrowAsync<Exception>().WithMessage($"Standard for ABCDE was not found");
        courseTypesApi.Verify(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()), Times.Never);
        coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Once);
    }
}