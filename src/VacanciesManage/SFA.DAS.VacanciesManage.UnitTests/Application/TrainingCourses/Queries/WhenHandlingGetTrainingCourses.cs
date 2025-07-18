﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;
using SFA.DAS.VacanciesManage.InnerApi.Responses;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenHandlingGetTrainingCourses
    {
        [Test, MoqAutoData]
        public async Task And_Courses_Returned_From_Service(
            GetTrainingCoursesQuery query,
            GetStandardsListResponse coursesFromCache,
            [Frozen] Mock<ICourseService> mockCacheService,
            GetTrainingCoursesQueryHandler handler)
        {
            mockCacheService
                .Setup(service => service.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(coursesFromCache);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingCourses.Should().BeEquivalentTo(coursesFromCache.Standards);
        }
    }
}