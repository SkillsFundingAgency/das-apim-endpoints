using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Validation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services;

public interface IAutoReservationsService
{
    public Task<Guid> CreateReservation(CreateCohortCommand command);
    public Task DeleteReservation(Guid id);
}

public class AutoReservationsService : IAutoReservationsService
{
    private readonly IReservationApiClient<ReservationApiConfiguration> _reservationsApiClient;
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
    private readonly ILogger<AutoReservationsService> _logger;

    public AutoReservationsService(IReservationApiClient<ReservationApiConfiguration> reservationsApiClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, ILogger<AutoReservationsService> logger)
    {
        _reservationsApiClient = reservationsApiClient;
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<Guid> CreateReservation(CreateCohortCommand command)
    {
        var id = Guid.NewGuid();
        Guid.TryParse(command.UserInfo.UserId, out var userId);

        if (command.TransferSenderId != null)
        {
            throw new ApplicationException("When creating a auto reservation, the TransferSenderId must be null");
        }

        _logger.LogInformation("Getting Account Legal Entity for {0}", command.AccountLegalEntityId);
        var ale = await _apiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(command.AccountLegalEntityId));
        if (ale == null)
        {
            throw new ApplicationException("When creating an auto reservation, the AccountLegalEntity was not found");
        }

        if (command.StartDate == null)
        {
            throw CreateApiModelException("StartDate", "You must enter a start date to reserve new funding");
        }

        _logger.LogInformation("Creating Reservation with id {0}", id);
        var request = new CreateReservationRequest
        {
            AccountId = command.AccountId,
            AccountLegalEntityId = command.AccountLegalEntityId,
            AccountLegalEntityName = ale.LegalEntityName,
            CourseId = command.CourseCode,
            IsLevyAccount = false,
            Id = id,
            ProviderId = (uint)command.ProviderId,
            StartDate = command.StartDate,
            TransferSenderAccountId = null,
            UserId = userId
        };

        var post = new PostCreateReservationRequest(request);
        var response = await _reservationsApiClient.PostWithResponseCode<CreateReservationResponse>(post);

        if (response.Body != null)
        {
            _logger.LogInformation("Reservation id {0} was created", id);
            return Guid.NewGuid(); // response.Body.Id;
        }

        var exception = ConvertToDomainOrApplicationException(response.ErrorContent);
        throw exception;
    }

    public Task DeleteReservation(Guid id)
    {
        return _reservationsApiClient.Delete(new DeleteReservationRequest(id));
    }

    private Exception ConvertToDomainOrApplicationException(string responseErrorContent)
    {
        _logger.LogInformation("Creating of reservation failed with this content {0}", responseErrorContent);

        var error = JsonConvert.DeserializeObject<ReservationArgumentErrorResponse>(responseErrorContent);

        if (error == null || !error.Message.StartsWith("The following parameters have failed validation", StringComparison.InvariantCultureIgnoreCase))
        {
            return new ApplicationException("Unexpected error when creating reservation");
        }

        if (error.Params.Contains("StartDate", StringComparison.CurrentCultureIgnoreCase))
        {
            var currentDate = DateTime.Today;
            var validFromDate = currentDate.AddMonths(-1).ToString("MM yyyy");
            var validToDate = currentDate.AddMonths(2).ToString("MM yyyy");
            var errorMessage = $"Training start date must be between the funding reservation dates {validFromDate} to {validToDate}";

            return CreateApiModelException("StartDate", errorMessage);
        }

        return new ApplicationException("Unexpected error when creating reservation"); ;
    }

    private Exception CreateApiModelException(string fieldName, string message)
    {
        _logger.LogInformation("Creation of reservation failed for field {0} with message {1}", fieldName, message);

        var errorDetail = new ErrorDetail(fieldName, message);
        var errorResponse = new ErrorResponse(new List<ErrorDetail> { errorDetail });

        var content = JsonConvert.SerializeObject(errorResponse);

        return new DomainApimException(content);
    }
}