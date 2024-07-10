using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.FindEpao.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Queries.GetCourseEpaos
{
    public class WhenHandlingGetCourseEpaosQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Epaos_From_Assessors_Api_And_Course_From_Courses_Api(
            GetCourseEpaosQuery query,
            List<GetCourseEpaoListItem> epaoApiResponse,
            GetStandardsListItem coursesApiResponse,
            IEnumerable<GetStandardsExtendedListItem> standardsResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseEpaoIsValidFilterService> mockCourseEpaoFilter,
            GetCourseEpaosQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetCourseEpaoListItem>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(epaoApiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(
                    It.Is<GetStandardRequest>(request => request.StandardId == query.CourseId)))
                .ReturnsAsync(coursesApiResponse);

            foreach (var epao in epaoApiResponse)
                mockAssessorsApiClient
                    .Setup(client => client.Get<IEnumerable<GetStandardsExtendedListItem>>(
                        It.Is<GetCourseEpaosStandardVersionsRequest>(request => request.LarsCode == query.CourseId &&
                        request.organisationId == epao.EpaoId)))
                    .ReturnsAsync(standardsResponse);
            mockCourseEpaoFilter
                .Setup(service => service.IsValidCourseEpao(It.IsAny<GetCourseEpaoListItem>()))
                .Returns(true);
            mockCourseEpaoFilter
                .Setup(service => service.ValidateVersionDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(true);


            var result = await handler.Handle(query, CancellationToken.None);
            result.Epaos.Select(x => x.CourseEpaoDetails.StandardVersions)
                .FirstOrDefault().Should().BeEquivalentTo(standardsResponse.Select(x => x.Version));
            result.Epaos.Should().BeEquivalentTo(epaoApiResponse);
            result.Epaos.Should().BeInAscendingOrder(item => item.Name);
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Epaos_From_Assessors_Api_And_Course_From_Courses_Api_And_Filters_Epaos(
            GetCourseEpaosQuery query,
            List<GetCourseEpaoListItem> epaoApiResponse,
            GetStandardsListItem coursesApiResponse,
            IEnumerable<GetStandardsExtendedListItem> standardsResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseEpaoIsValidFilterService> mockCourseEpaoFilter,
            GetCourseEpaosQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetCourseEpaoListItem>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(epaoApiResponse);

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(
                    It.Is<GetStandardRequest>(request => request.StandardId == query.CourseId)))
                .ReturnsAsync(coursesApiResponse);

            foreach (var epao in epaoApiResponse)
                mockAssessorsApiClient
                .Setup(client => client.Get<IEnumerable<GetStandardsExtendedListItem>>(
                    It.Is<GetCourseEpaosStandardVersionsRequest>(request => request.LarsCode == query.CourseId &&
                    request.organisationId == epao.EpaoId)))
                .ReturnsAsync(standardsResponse);

            mockCourseEpaoFilter
                .Setup(service => service.IsValidCourseEpao(It.Is<GetCourseEpaoListItem>(item => item.EpaoId == epaoApiResponse[0].EpaoId)))
                .Returns(true);
            mockCourseEpaoFilter
                .Setup(service => service.ValidateEpaoStandardDates(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(true);
            mockCourseEpaoFilter
                .Setup(service => service.ValidateVersionDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(true);
           
            var result = await handler.Handle(query, CancellationToken.None);

            result.Epaos.Select(x => x.CourseEpaoDetails.StandardVersions)
                .FirstOrDefault().Should().BeEquivalentTo(standardsResponse.Select(x => x.Version));
            result.Epaos.Should().BeEquivalentTo(epaoApiResponse.Where(item => item.EpaoId == epaoApiResponse[0].EpaoId).ToList());
            result.Epaos.Should().BeInAscendingOrder(item => item.Name);
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
        }


        [Test, MoqAutoData]
        public async Task If_No_Valid_Standards_Found_Returns_Zero_Epaos(
        GetCourseEpaosQuery query,
        List<GetCourseEpaoListItem> epaoApiResponse,
        GetStandardsListItem coursesApiResponse,
        IEnumerable<GetStandardsExtendedListItem> standardsResponse,
        [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
        [Frozen] Mock<ICourseEpaoIsValidFilterService> mockCourseEpaoFilter,
        GetCourseEpaosQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetCourseEpaoListItem>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(epaoApiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(
                    It.Is<GetStandardRequest>(request => request.StandardId == query.CourseId)))
                .ReturnsAsync(coursesApiResponse);
            foreach (var epao in epaoApiResponse)
                mockAssessorsApiClient
                    .Setup(client => client.Get<IEnumerable<GetStandardsExtendedListItem>>(
                        It.Is<GetCourseEpaosStandardVersionsRequest>(request => request.LarsCode == query.CourseId &&
                        request.organisationId == epao.EpaoId)))
                    .ReturnsAsync(standardsResponse);
            mockCourseEpaoFilter
                .Setup(service => service.IsValidCourseEpao(It.IsAny<GetCourseEpaoListItem>()))
                .Returns(true);
            mockCourseEpaoFilter
                .Setup(service => service.ValidateVersionDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(false);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epaos.Should().BeEmpty();
        }



    }
}