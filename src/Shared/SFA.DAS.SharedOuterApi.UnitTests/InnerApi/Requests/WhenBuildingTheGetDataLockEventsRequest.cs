using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetDataLockEventsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
         long SinceEventId,
         DateTime? SinceTime,
         string EmployerAccountId,
         long Ukprn,
         int PageNumber)
        {
            var actual = new GetDataLockEventsRequest
            {
                EmployerAccountId = EmployerAccountId,
                Ukprn = Ukprn,
                PageNumber = PageNumber,
                SinceEventId = SinceEventId,
                SinceTime = SinceTime,
            };

            actual.GetUrl.Should()
                .Be($"api/v2/datalock?page={PageNumber}&sinceEventId={SinceEventId}&sinceTime={SinceTime.Value:yyyy-MM-ddTHH:mm:ss}&employerAccountId={EmployerAccountId}&ukprn={Ukprn}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_SinceEventId(
          long SinceEventId,
          DateTime? SinceTime,
          string EmployerAccountId,
          long Ukprn,
          int PageNumber)
        {
            var actual = new GetDataLockEventsRequest
            {
                EmployerAccountId = EmployerAccountId,
                Ukprn = Ukprn,
                PageNumber = PageNumber,
                SinceTime = SinceTime,
            };

            actual.GetUrl.Should()
                .Be($"api/v2/datalock?page={PageNumber}&sinceTime={SinceTime.Value:yyyy-MM-ddTHH:mm:ss}&employerAccountId={EmployerAccountId}&ukprn={Ukprn}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_SinceTime(
         long SinceEventId,
         DateTime? SinceTime,
         string EmployerAccountId,
         long Ukprn,
         int PageNumber)
        {
            var actual = new GetDataLockEventsRequest
            {
                EmployerAccountId = EmployerAccountId,
                Ukprn = Ukprn,
                PageNumber = PageNumber,
                SinceEventId = SinceEventId,
            };

            actual.GetUrl.Should()
          .Be($"api/v2/datalock?page={PageNumber}&sinceEventId={SinceEventId}&employerAccountId={EmployerAccountId}&ukprn={Ukprn}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_EmployerAccountId(
        long SinceEventId,
        DateTime? SinceTime,
        string EmployerAccountId,
        long Ukprn,
        int PageNumber)
        {
            var actual = new GetDataLockEventsRequest
            {
                Ukprn = Ukprn,
                PageNumber = PageNumber,
                SinceEventId = SinceEventId,
                SinceTime = SinceTime,
            };

            actual.GetUrl.Should()
                .Be($"api/v2/datalock?page={PageNumber}&sinceEventId={SinceEventId}&sinceTime={SinceTime.Value:yyyy-MM-ddTHH:mm:ss}&ukprn={Ukprn}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_Ukprn(
        long SinceEventId,
        DateTime? SinceTime,
        string EmployerAccountId,
        long Ukprn,
        int PageNumber)
        {
            var actual = new GetDataLockEventsRequest
            {
                EmployerAccountId = EmployerAccountId,
                PageNumber = PageNumber,
                SinceEventId = SinceEventId,
                SinceTime = SinceTime,
            };

            actual.GetUrl.Should()
                .Be($"api/v2/datalock?page={PageNumber}&sinceEventId={SinceEventId}&sinceTime={SinceTime.Value:yyyy-MM-ddTHH:mm:ss}&employerAccountId={EmployerAccountId}");
        }
    }
}