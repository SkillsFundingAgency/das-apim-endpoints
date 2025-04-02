﻿using SFA.DAS.Recruit.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingGetApplicationReviewsCountByAccountIdApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(long accountId, List<long> vacancyReferences)
        {
            //Act
            var actual = new GetApplicationReviewsCountByAccountIdApiRequest(accountId, vacancyReferences);

            //Assert
            actual.PostUrl.Should().Be($"api/employer/{accountId}/applicationReviews/count");
        }
    }
}
