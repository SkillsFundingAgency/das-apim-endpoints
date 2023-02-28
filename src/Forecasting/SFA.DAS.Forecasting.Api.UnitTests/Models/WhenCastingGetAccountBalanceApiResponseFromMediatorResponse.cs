using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance;

namespace SFA.DAS.Forecasting.Api.UnitTests.Models
{
    public class WhenCastingGetAccountBalanceApiResponseFromMediatorResponse
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped(GetAccountBalanceQueryResult source)
        {
            var actual = (GetAccountBalanceApiResponse) source;
            
            actual.Should().BeEquivalentTo(source.AccountBalance, options=> options.Excluding(c=>c.AccountId));
        }

        [Test, AutoData]
        public void Then_If_Null_Then_Null_Returned(GetAccountBalanceQueryResult source)
        {
            source.AccountBalance = null;
            
            var actual = (GetAccountBalanceApiResponse) source;

            actual.Should().BeNull();
        }
    }
}