using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Validation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Services.AutoReservationsServiceTests
{
    public class AutoReservationServiceTests
    {
        [Test]
        public async Task When_Creating_A_Reservation_And_No_LegalEntity_Found_Then_Throw_Exception()
        {
            var fixture = new AutoReservationsServiceTestFixture();
            Func<Task> act = async () => await fixture.Sut.CreateReservation(fixture.AutoReservation);

            await act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("When creating an auto reservation, the AccountLegalEntity was not found");
        }

        [Test]
        public async Task When_Creating_A_Reservation_It_returns_ReservationId_Successfully()
        {
            var fixture = new AutoReservationsServiceTestFixture().WithAccountLegalEntity();
            var result = await fixture.Sut.CreateReservation(fixture.AutoReservation);

            result.Should().Be(fixture.CreateReservationResponse.Id);
            fixture.ReservationsApiClient.Verify(x=>x.PostWithResponseCode<CreateReservationResponse>(It.IsAny<PostCreateReservationRequest>(), true));
        }

        [Test]
        public async Task When_Creating_A_Reservation_And_It_Returns_An_Unhandled_Exception()
        {
            var fixture = new AutoReservationsServiceTestFixture().WithAccountLegalEntity().WithErrorContent("Some Random text");
            var act = async () => await fixture.Sut.CreateReservation(fixture.AutoReservation);

            act.Should().ThrowAsync<ApplicationException>().WithMessage("Unexpected error when creating reservation");
        }

        [Test]
        public async Task When_Creating_A_Reservation_And_It_Returns_A_CourseId_Exception()
        {
            var fixture = new AutoReservationsServiceTestFixture().WithAccountLegalEntity()
                .WithErrorContent("Some error message (CourseId) text");

            try
            {
                await fixture.Sut.CreateReservation(fixture.AutoReservation);
                Assert.Fail("Should fail");
            }
            catch (DomainApimException ex)
            {
                ex.Content.Should().Contain("Funding is not available for this course on this start date");
            }
        }

        [Test]
        public async Task When_Creating_A_Reservation_And_It_Returns_An_Empty_ErrorArray()
        {
            var error = new ReservationsStartDateErrorResponse {StartDate = new List<string>()};

            var fixture = new AutoReservationsServiceTestFixture().WithAccountLegalEntity().WithErrorContent(JsonConvert.SerializeObject(error));
            var act = async () => await fixture.Sut.CreateReservation(fixture.AutoReservation);

            await act.Should().ThrowAsync<DomainApimException>();
        }

        [Test]
        public async Task When_Creating_A_Reservation_And_It_Returns_An_Empty_First_Element()
        {
            var error = new ReservationsStartDateErrorResponse { StartDate = new List<string> {""} };

            var fixture = new AutoReservationsServiceTestFixture().WithAccountLegalEntity().WithErrorContent(JsonConvert.SerializeObject(error));
            var act = async () => await fixture.Sut.CreateReservation(fixture.AutoReservation);

            await act.Should().ThrowAsync<DomainApimException>();
        }

        [Test]
        public async Task When_Creating_A_Reservation_And_It_Returns_Error_In_StartDate()
        {
            var error = new ReservationsStartDateErrorResponse { StartDate = new List<string> { "Date Error" } };

            var fixture = new AutoReservationsServiceTestFixture().WithAccountLegalEntity().WithErrorContent(JsonConvert.SerializeObject(error));
            var act = async () => await fixture.Sut.CreateReservation(fixture.AutoReservation);

            await act.Should().ThrowAsync<DomainApimException>();
        }

        private class AutoReservationsServiceTestFixture
        {
            public readonly Mock<IReservationApiClient<ReservationApiConfiguration>> ReservationsApiClient;
            public readonly Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> CommitmentsApiClient;
            public AutoReservation AutoReservation;
            public IAutoReservationsService Sut;
            public GetAccountLegalEntityResponse GetAccountLegalEntityResponse;
            public CreateReservationResponse CreateReservationResponse;
            public PostCreateReservationRequest PostCreateReservationRequest;
            public CreateReservationRequest CreateReservationRequest;

            public AutoReservationsServiceTestFixture()
            {
                var fixture = new Fixture();

                ReservationsApiClient = new Mock<IReservationApiClient<ReservationApiConfiguration>>();
                CommitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
                AutoReservation = fixture.Create<AutoReservation>();
                GetAccountLegalEntityResponse = fixture.Create<GetAccountLegalEntityResponse>();
                CreateReservationResponse = fixture.Create<CreateReservationResponse>();

                ReservationsApiClient
                    .Setup(x => x.PostWithResponseCode<CreateReservationResponse>(It.IsAny<IPostApiRequest>(), true))
                    .ReturnsAsync(new ApiResponse<CreateReservationResponse>(CreateReservationResponse, HttpStatusCode.OK, null));

                Sut = new AutoReservationsService(ReservationsApiClient.Object, CommitmentsApiClient.Object, Mock.Of<ILogger<AutoReservationsService>>());
            }

            public AutoReservationsServiceTestFixture WithAccountLegalEntity()
            {
                CommitmentsApiClient
                    .Setup(x => x.Get<GetAccountLegalEntityResponse>(It.Is<GetAccountLegalEntityRequest>(p =>
                        p.AccountLegalEntityId == AutoReservation.AccountLegalEntityId)))
                    .ReturnsAsync(GetAccountLegalEntityResponse);

                return this;
            }

            public AutoReservationsServiceTestFixture WithErrorContent(string errorContent)
            {
                ReservationsApiClient
                    .Setup(x => x.PostWithResponseCode<CreateReservationResponse>(It.IsAny<IPostApiRequest>(), true))
                    .ReturnsAsync(new ApiResponse<CreateReservationResponse>(null, HttpStatusCode.BadRequest, errorContent));

                return this;
            }
        }
    }
}