﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostRenewSubscriptionKeyRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountIdentifier, string productId)
        {
            var actual = new PostRenewSubscriptionKeyRequest(accountIdentifier, productId);

            actual.PostUrl.Should().Be($"api/subscription/renew");
            actual.Data.Should().NotBeNull();
            actual.Data.AccountIdentifier.Should().Be(accountIdentifier);
            actual.Data.ProductId.Should().Be(productId);
        }
    }
}