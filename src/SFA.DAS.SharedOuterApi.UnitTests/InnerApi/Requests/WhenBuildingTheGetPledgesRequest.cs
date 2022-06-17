﻿using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetPledgesRequest
    {
        [Test, AutoData]
        public void And_No_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built()
        {
            var actual = new GetPledgesRequest();

            Assert.AreSame(
                $"pledges",
                actual.GetUrl);
        }

        [Test, AutoData]
        public void And_The_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built(long accountId)
        {
            var actual = new GetPledgesRequest(accountId: accountId);

            Assert.AreEqual(
                $"pledges?accountId={accountId}",
                actual.GetUrl);
        }
    }
}