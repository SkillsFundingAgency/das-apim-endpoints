using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.AddressLookup
{
    [TestFixture]
    public class AddressItemTests
    {
        [Test, AutoData]
        public void Operator_ConvertsToAddressItem(GetAddressesListItem source)
        {
            var addressItem = (AddressItem)source;

            addressItem.Should().BeEquivalentTo(source, option =>
            {
                option.ExcludingMissingMembers();
                option.WithMapping<AddressItem>(s => s.PostTown, t => t.Town);
                return option;
            });
        }
    }
}
