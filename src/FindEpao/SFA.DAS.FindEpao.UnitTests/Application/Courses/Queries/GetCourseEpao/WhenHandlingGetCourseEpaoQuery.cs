using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.FindEpao.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Validation;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.SharedOuterApi.Validation.ValidationResult;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Queries.GetCourseEpao
{
    public class WhenHandlingGetCourseEpaoQuery
    {
        [Test, MoqAutoData]
        public void And_Validation_Error_Then_Throws_ValidationException(
            GetCourseEpaoQuery query,
            string propertyName,
            ValidationResult validationResult,
            [Frozen] Mock<IValidator<GetCourseEpaoQuery>> mockValidator,
            GetCourseEpaoQueryHandler handler)
        {
            validationResult.AddError(propertyName);
            mockValidator
                .Setup(validator => validator.ValidateAsync(It.IsAny<GetCourseEpaoQuery>()))
                .ReturnsAsync(validationResult);

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public void And_No_Epao_Courses_Then_Throws_NotFoundException(
            GetCourseEpaoQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCourseEpaoQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(default(GetEpaoResponse));

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<NotFoundException<GetCourseEpaoResult>>();
        }

        [Test, MoqAutoData]
        public void And_Epao_Course_Not_Valid_Then_Throws_NotFoundException(
            GetCourseEpaoQuery query,
            GetEpaoResponse epaoApiResponse,
            List<GetCourseEpaoListItem> courseEpaosApiResponse,
            List<GetDeliveryAreaListItem> areasFromCache,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICachedDeliveryAreasService> mockCacheService,
            [Frozen] Mock<ICourseEpaoIsValidFilterService> mockCourseEpaoFilter,
            GetCourseEpaoQueryHandler handler)
        {
            courseEpaosApiResponse[0].EpaoId = query.EpaoId.ToLower();
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoApiResponse);
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetCourseEpaoListItem>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(courseEpaosApiResponse);
            mockCacheService
                .Setup(service => service.GetDeliveryAreas())
                .ReturnsAsync(areasFromCache);
            mockCourseEpaoFilter
                .Setup(service => service.IsValidCourseEpao(It.IsAny<GetCourseEpaoListItem>()))
                .Returns(false);

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<NotFoundException<GetCourseEpaoResult>>();
        }

        [Test, MoqAutoData]
        public async Task If_No_Valid_Standards_Found_Returns_Zero_AllCourses(
            GetCourseEpaoQuery query,
            GetEpaoResponse epaoApiResponse,
            List<GetCourseEpaoListItem> courseEpaosApiResponse,
            List<GetEpaoCourseListItem> epaoCoursesApiResponse,
            List<GetDeliveryAreaListItem> areasFromCache,
            GetStandardsListResponse coursesFromCache,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICachedDeliveryAreasService> mockCachedAreasService,
            [Frozen] Mock<ICachedCoursesService> mockCachedCoursesService,
            [Frozen] Mock<ICourseEpaoIsValidFilterService> mockCourseEpaoFilter,
            GetCourseEpaoQueryHandler handler)
        {
            courseEpaosApiResponse[0].EpaoId = query.EpaoId.ToLower();
            coursesFromCache.Standards.First().LarsCode = epaoCoursesApiResponse.First().StandardCode;
            coursesFromCache.Standards.ElementAt(1).LarsCode = query.CourseId;
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoApiResponse);
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetCourseEpaoListItem>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(courseEpaosApiResponse);
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCourseListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoCoursesApiResponse);
            mockCachedAreasService
                .Setup(service => service.GetDeliveryAreas())
                .ReturnsAsync(areasFromCache);
            mockCachedCoursesService
                .Setup(client => client.GetCourses())
                .ReturnsAsync(coursesFromCache);
            mockCourseEpaoFilter
                .Setup(service => service.IsValidCourseEpao(It.IsAny<GetCourseEpaoListItem>()))
                .Returns<GetCourseEpaoListItem>(item => item.EpaoId == query.EpaoId.ToLower());
            mockCourseEpaoFilter
                .Setup(service => service.ValidateEpaoStandardDates(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(true);
            mockCourseEpaoFilter
                .Setup(service => service.ValidateVersionDates(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(false);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epao.Should().BeEquivalentTo(epaoApiResponse);
            result.StandardVersions.Should().BeEmpty();
            result.AllCourses.Should().BeEmpty();
        }


        [Test, MoqAutoData]
        public async Task Then_Gets_Epao_From_Assessors_Api_And_Course_From_Courses_Api_And_DeliveryAreas_From_Cache_And_Other_Courses_From_Cache_Filtered(
            GetCourseEpaoQuery query,
            GetEpaoResponse epaoApiResponse,
            List<GetCourseEpaoListItem> courseEpaosApiResponse,
            List<GetEpaoCourseListItem> epaoCoursesApiResponse,
            List<GetDeliveryAreaListItem> areasFromCache,
            GetStandardsListResponse coursesFromCache,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICachedDeliveryAreasService> mockCachedAreasService,
            [Frozen] Mock<ICachedCoursesService> mockCachedCoursesService,
            [Frozen] Mock<ICourseEpaoIsValidFilterService> mockCourseEpaoFilter,
            GetCourseEpaoQueryHandler handler)
        {
            courseEpaosApiResponse[0].EpaoId = query.EpaoId.ToLower();
            coursesFromCache.Standards.First().LarsCode = epaoCoursesApiResponse.First().StandardCode;
            coursesFromCache.Standards.ElementAt(1).LarsCode = query.CourseId;
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoApiResponse);
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetCourseEpaoListItem>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(courseEpaosApiResponse);
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCourseListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoCoursesApiResponse);
            mockCachedAreasService
                .Setup(service => service.GetDeliveryAreas())
                .ReturnsAsync(areasFromCache);
            mockCachedCoursesService
                .Setup(client => client.GetCourses())
                .ReturnsAsync(coursesFromCache);
            mockCourseEpaoFilter
                .Setup(service => service.IsValidCourseEpao(It.IsAny<GetCourseEpaoListItem>()))
                .Returns<GetCourseEpaoListItem>(item => item.EpaoId == query.EpaoId.ToLower());
            mockCourseEpaoFilter
                .Setup(service => service.ValidateEpaoStandardDates(It.IsAny<DateTime?>(),It.IsAny<DateTime?>(),It.IsAny<DateTime?>()))
                .Returns(true);
            mockCourseEpaoFilter
                .Setup(service => service.ValidateVersionDates(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(true);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epao.Should().BeEquivalentTo(epaoApiResponse);
            result.EpaoDeliveryAreas.Should().BeEquivalentTo(courseEpaosApiResponse.Single(item => string.Equals(item.EpaoId, query.EpaoId, StringComparison.CurrentCultureIgnoreCase)).DeliveryAreas);
            result.CourseEpaosCount.Should().Be(courseEpaosApiResponse.Count(item => item.EpaoId == query.EpaoId.ToLower()));//filter returns true
            result.Course.Should().BeEquivalentTo(coursesFromCache.Standards.Single(item => item.LarsCode == query.CourseId));
            result.DeliveryAreas.Should().BeEquivalentTo(areasFromCache);
            result.AllCourses.Should().BeEquivalentTo(
                coursesFromCache.Standards.Where(item =>
                    epaoCoursesApiResponse.Any(listItem => listItem.StandardCode == query.CourseId)));

            foreach (var courseListItem in epaoCoursesApiResponse)
            {
                mockCourseEpaoFilter.Verify(x=>x.ValidateEpaoStandardDates(courseListItem.DateStandardApprovedOnRegister,courseListItem.EffectiveTo,courseListItem.EffectiveFrom), 
                    Times.Once);

                foreach (var standardVersion in courseListItem.StandardVersions)
                    mockCourseEpaoFilter.Verify(x => x.ValidateVersionDates(standardVersion.EffectiveFrom, standardVersion.EffectiveTo), Times.Once);
            }

            result.EffectiveFrom.Should().Be(courseEpaosApiResponse
                .Single(item => item.EpaoId == query.EpaoId.ToLower())
                .CourseEpaoDetails.EffectiveFrom!.Value);//nulls removed in filter


        }
    }
}
