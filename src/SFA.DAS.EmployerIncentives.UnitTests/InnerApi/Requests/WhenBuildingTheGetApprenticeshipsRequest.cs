using System;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetApprenticeshipsRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long accountId, long employerAccountId, DateTime startDateFrom, DateTime startDateTo, int pageNumber, int pageSize)
        {
            var actual = new GetApprenticeshipsRequest(accountId, employerAccountId, startDateFrom, startDateTo, pageNumber, pageSize);

            actual.GetUrl.Should()
                .Be($"api/apprenticeships?accountId={accountId}&accountLegalEntityId={employerAccountId}&startDateRangeFrom={WebUtility.UrlEncode(startDateFrom.ToString("u"))}&startDateRangeTo={WebUtility.UrlEncode(startDateTo.ToString("u"))}&pageNumber={pageNumber}&pageItemCount={pageSize}&sortField=ApprenticeName&status=1");
        }
    }
}