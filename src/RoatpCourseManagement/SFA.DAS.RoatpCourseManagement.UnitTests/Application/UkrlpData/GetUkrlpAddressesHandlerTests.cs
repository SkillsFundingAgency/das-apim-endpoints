using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;
using SFA.DAS.RoatpCourseManagement.Configuration;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.UkrlpData
{
    [TestFixture]
    public class GetUkrlpAddressesHandlerTests
    {
        private const string LegalIdentifier = "L";
        private const string QueryId = "2";
        private const string StakeholderId = "3";


        [Test]
        public async Task GetProviderAddressesUsingProviderUpdatedSince_OkResponse_ReturnsContent()
        {
            var content = "xml that comes back";
            var ukprn1 = "12345678";
            var ukprn2 = "87654321";
            var address1_1 = "1 green street";
            var address2_1 = "centre";
            var address3_1 = "New area";
            var address4_1 = "Smallville";
            var town_1 = "New Town";
            var postcode_1 = "ZZ1 1ZZ";
            var address1_2 = "2 green street";
            var request = "string request";
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            var matchingProviderRecords = new List<Provider>
            {
                new()
                {
                    UnitedKingdomProviderReferenceNumber = ukprn1,
                    ProviderContacts = new List<ProviderContact>
                    {
                        new()
                        {
                            ContactType = "X",
                            ContactAddress = new ProviderContactAddress
                            {
                                Address1 = "not a legal address"
                            }
                        },
                        new()
                        {
                            ContactType = LegalIdentifier,
                            ContactAddress = new ProviderContactAddress
                            {
                                Address1 = address1_1,
                                Address2 = address2_1,
                                Address3 = address3_1,
                                Address4 = address4_1,
                                Town = town_1,
                                PostCode = postcode_1
                            }
                        }
                    }
                },
                new()
                {
                    UnitedKingdomProviderReferenceNumber = ukprn2,
                    ProviderContacts = new List<ProviderContact>
                    {
                        new()
                        {
                            ContactType = LegalIdentifier,
                            ContactAddress = new ProviderContactAddress
                            {
                                Address1 = address1_2
                            }
                        }
                    }
                }
            };

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content),
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");
            var mockSerializer = new Mock<IUkrlpSoapSerializer>();

            mockSerializer
                .Setup(x => x.BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime.Today, StakeholderId, QueryId))
                .Returns(request);

            mockSerializer
                .Setup(x => x.DeserialiseMatchingProviderRecordsResponse(content))
                .Returns(matchingProviderRecords);

            var ukrlpApiConfiguration = new UkrlpApiConfiguration
            {
                ApiBaseAddress = "https://test",
                QueryId = QueryId,
                StakeholderId = StakeholderId
            };

            var optionsConfiguration = Options.Create(ukrlpApiConfiguration);

            var sut = new GetUkrlpAddressesHandler(mockSerializer.Object, httpClient, Mock.Of<ILogger<GetUkrlpAddressesHandler>>(), optionsConfiguration);

            var command = new UkrlpDataCommand
            {
                ProvidersUpdatedSince = DateTime.Today
            };

            var response = await sut.Handle(command, new CancellationToken());

            var addresses = response.Results;

            addresses.Count.Should().Be(2);

            var address1 = addresses.First(x => x.Ukprn == ukprn1);
            address1.Address1.Should().Be(address1_1);
            address1.Address2.Should().Be(address2_1);
            address1.Address3.Should().Be(address3_1);
            address1.Address4.Should().Be(address4_1);
            address1.Town.Should().Be(town_1);
            address1.Postcode.Should().Be(postcode_1);

            var address2 = addresses.First(x => x.Ukprn.ToString() == ukprn2);

            address2.Address1.Should().Be(address1_2);
            mockSerializer.Verify(x => x.BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime.Today, StakeholderId, QueryId), Times.Once);
            mockSerializer.Verify(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(It.IsAny<List<long>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task GetProviderAddressesUsingUkprns_OkResponse_ReturnsContent()
        {
            var content = "xml that comes back";
            long ukprn1 = 12345678;
            long ukprn2 = 87654321;
            var address1_1 = "1 green street";
            var address2_1 = "centre";
            var address3_1 = "New area";
            var address4_1 = "Smallville";
            var town_1 = "New Town";
            var postcode_1 = "ZZ1 1ZZ";
            var address1_2 = "2 green street";
            var request = "string request";
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            var matchingProviderRecords = new List<Provider>
        {
            new()
            {
                UnitedKingdomProviderReferenceNumber = ukprn1.ToString(),
                ProviderContacts = new List<ProviderContact>
                {
                    new()
                    {
                        ContactType = "X",
                        ContactAddress = new ProviderContactAddress
                        {
                            Address1 = "not a legal address"
                        }
                    },
                    new()
                    {
                        ContactType = LegalIdentifier,
                        ContactAddress = new ProviderContactAddress
                        {
                            Address1 = address1_1,
                            Address2 = address2_1,
                            Address3 = address3_1,
                            Address4 = address4_1,
                            Town = town_1,
                            PostCode = postcode_1
                        }
                    }
                }
            },
            new()
            {
                UnitedKingdomProviderReferenceNumber = ukprn2.ToString(),
                ProviderContacts = new List<ProviderContact>
                {
                    new()
                    {
                        ContactType = LegalIdentifier,
                        ContactAddress = new ProviderContactAddress
                        {
                            Address1 = address1_2
                        }
                    }
                }
            }
        };

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content),
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");
            var mockSerializer = new Mock<IUkrlpSoapSerializer>();

            mockSerializer
                .Setup(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(new List<long> { ukprn1, ukprn2 }, StakeholderId, QueryId))
                .Returns(request);

            mockSerializer
                .Setup(x => x.DeserialiseMatchingProviderRecordsResponse(content))
                .Returns(matchingProviderRecords);

            var ukrlpApiConfiguration = new UkrlpApiConfiguration
            {
                ApiBaseAddress = "https://test",
                QueryId = QueryId,
                StakeholderId = StakeholderId
            };

            var optionsConfiguration = Options.Create(ukrlpApiConfiguration);

            var sut = new GetUkrlpAddressesHandler(mockSerializer.Object, httpClient, Mock.Of<ILogger<GetUkrlpAddressesHandler>>(), optionsConfiguration);

            var command = new UkrlpDataCommand
            {
                Ukprns = new List<long>
            {
                ukprn1,
                ukprn2
            }
            };

            var response = await sut.Handle(command, new CancellationToken());
            var addresses = response.Results;

            addresses.Count.Should().Be(2);

            var address1 = addresses.First(x => x.Ukprn == ukprn1.ToString());
            address1.Address1.Should().Be(address1_1);
            address1.Address2.Should().Be(address2_1);
            address1.Address3.Should().Be(address3_1);
            address1.Address4.Should().Be(address4_1);
            address1.Town.Should().Be(town_1);
            address1.Postcode.Should().Be(postcode_1);

            var address2 = addresses.First(x => x.Ukprn == ukprn2.ToString());

            address2.Address1.Should().Be(address1_2);
            mockSerializer.Verify(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(new List<long> { ukprn1, ukprn2 }, StakeholderId, QueryId), Times.Once);
            mockSerializer.Verify(x => x.BuildGetAllUkrlpsUpdatedSinceSoapRequest(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task GetProviderAddressesUsing501UkprnsCallsClientTwice_OkResponse_ReturnsContent()
        {
            var content = "xml that comes back";
            var request = "string request 500";
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            var matchingProviderRecords = new List<Provider>();
            var listOfUkprns = new List<long>();
            var rnd = new Random();
            for (var i = 0; i < 501; i++)
            {
                var ukprn = rnd.Next(10000000, 99999999);
                listOfUkprns.Add(ukprn);
                matchingProviderRecords.Add(new Provider { ProviderName = "Company " + i.ToString(), UnitedKingdomProviderReferenceNumber = ukprn.ToString(), ProviderContacts = new List<ProviderContact> { new ProviderContact { ContactType = LegalIdentifier, ContactAddress = new ProviderContactAddress { Address1 = i.ToString() + " Green road" } } } });
            }

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content),
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");
            var mockSerializer = new Mock<IUkrlpSoapSerializer>();

            mockSerializer
                .Setup(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(It.IsAny<List<long>>(), StakeholderId, QueryId))
                .Returns(request);

            mockSerializer
                .Setup(x => x.DeserialiseMatchingProviderRecordsResponse(content))
                .Returns(matchingProviderRecords);

            var ukrlpApiConfiguration = new UkrlpApiConfiguration
            {
                ApiBaseAddress = "https://test",
                QueryId = QueryId,
                StakeholderId = StakeholderId
            };

            var optionsConfiguration = Options.Create(ukrlpApiConfiguration);

            var sut = new GetUkrlpAddressesHandler(mockSerializer.Object, httpClient, Mock.Of<ILogger<GetUkrlpAddressesHandler>>(), optionsConfiguration);

            var command = new UkrlpDataCommand
            {
                Ukprns = listOfUkprns
            };

            var response = await sut.Handle(command, new CancellationToken());
            var addresses = response.Results;
            addresses.Count.Should().Be(1002);

            mockSerializer.Verify(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(It.IsAny<List<long>>(), StakeholderId, QueryId), Times.Exactly(2));
            mockSerializer.Verify(x => x.BuildGetAllUkrlpsUpdatedSinceSoapRequest(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task GetProviderAddressesWithProvidersUpdatedSince_UnsuccessfulResponse_ReturnsNull()
        {
            var request = "string request";
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");

            var ukrlpApiConfiguration = new UkrlpApiConfiguration
            {
                ApiBaseAddress = "https://test",
                QueryId = QueryId,
                StakeholderId = StakeholderId
            };

            var optionsConfiguration = Options.Create(ukrlpApiConfiguration);
            var mockSerializer = new Mock<IUkrlpSoapSerializer>();

            var sut = new GetUkrlpAddressesHandler(mockSerializer.Object, httpClient, Mock.Of<ILogger<GetUkrlpAddressesHandler>>(), optionsConfiguration);

            var command = new UkrlpDataCommand
            {
                ProvidersUpdatedSince = DateTime.Today
            };

            mockSerializer
                .Setup(x => x.BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime.Today, StakeholderId, QueryId))
                .Returns(request);

            var response = await sut.Handle(command, new CancellationToken());
            var addresses = response?.Results;
            addresses.Should().BeNull();
        }

        [Test]
        public async Task GetProviderAddressesWithUkprns_UnsuccessfulResponse_ReturnsNull()
        {
            var request = "string request";
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");

            var ukrlpApiConfiguration = new UkrlpApiConfiguration
            {
                ApiBaseAddress = "https://test",
                QueryId = QueryId,
                StakeholderId = StakeholderId
            };

            var optionsConfiguration = Options.Create(ukrlpApiConfiguration);
            var mockSerializer = new Mock<IUkrlpSoapSerializer>();

            var sut = new GetUkrlpAddressesHandler(mockSerializer.Object, httpClient, Mock.Of<ILogger<GetUkrlpAddressesHandler>>(), optionsConfiguration);

            var command = new UkrlpDataCommand
            {
                ProvidersUpdatedSince = null,
                Ukprns = new List<long> { 12345678 }
            };

            mockSerializer
                .Setup(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(command.Ukprns, StakeholderId, QueryId))
                .Returns(request);

            var response = await sut.Handle(command, new CancellationToken());

            response.Results.Should().BeEmpty();
            response.Success.Should().BeFalse();
        }

        [Test]
        public async Task GetProviderAddresses_NoMatchingRecords_OkResponse_ReturnsNoContent()
        {
            var content = "xml that comes back";
            var request = "string request";
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content),
                    RequestMessage = new HttpRequestMessage()
                });
            HttpClient httpClient = new HttpClient(mockMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://test");
            var mockSerializer = new Mock<IUkrlpSoapSerializer>();

            mockSerializer
                .Setup(x => x.BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime.Today, StakeholderId, QueryId))
                .Returns(request);

            mockSerializer
                .Setup(x => x.DeserialiseMatchingProviderRecordsResponse(content))
                .Returns((List<Provider>)null);

            var ukrlpApiConfiguration = new UkrlpApiConfiguration
            {
                ApiBaseAddress = "https://test",
                QueryId = QueryId,
                StakeholderId = StakeholderId
            };

            var optionsConfiguration = Options.Create(ukrlpApiConfiguration);

            var sut = new GetUkrlpAddressesHandler(mockSerializer.Object, httpClient, Mock.Of<ILogger<GetUkrlpAddressesHandler>>(), optionsConfiguration);

            var command = new UkrlpDataCommand
            {
                ProvidersUpdatedSince = DateTime.Today
            };

            var response = await sut.Handle(command, new CancellationToken());
            var addresses = response.Results;

            addresses.Count.Should().Be(0);
            mockSerializer.Verify(x => x.BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime.Today, StakeholderId, QueryId), Times.Once);
            mockSerializer.Verify(x => x.BuildGetAllUkrlpsFromUkprnsSoapRequest(It.IsAny<List<long>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

    }
}
