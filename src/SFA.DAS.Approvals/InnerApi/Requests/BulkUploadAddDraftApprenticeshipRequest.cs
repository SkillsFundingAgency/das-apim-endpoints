using System;
using System.Globalization;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadAddDraftApprenticeshipRequest
    {
        public string EndDateAsString { get; set; }
        public string CostAsString { get; set; }
        public int RowNumber { get; set; }
        public string ProviderRef { get; set; }
        public string Uln { get; set; }
        public string DateOfBirthAsString { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Guid? ReservationId { get; set; }
        public string OriginatorReference { get; set; }
        public string EPAOrgId { get; set; }
        public string StartDateAsString { get; set; }
        public string CourseCode { get; set; }
        public long ProviderId { get; set; }
        public string CohortRef { get; set; }
        public long? CohortId { get; set; }
        public string AgreementId { get; set; }
        public long? LegalEntityId { get; set; }
        public long? TransferSenderId { get; set; }

        public static implicit operator ReservationRequest(BulkUploadAddDraftApprenticeshipRequest response)
        {
            var legalEntityId = response.LegalEntityId.HasValue ? response.LegalEntityId.Value : 0;
            return new ReservationRequest
            {
                CourseId = response.CourseCode,
                AccountLegalEntityId = legalEntityId,
                ProviderId = (uint?)response.ProviderId,
                RowNumber = response.RowNumber,
                Id = Guid.NewGuid(),
                StartDate = GetStartDate(response.StartDateAsString),
                TransferSenderAccountId = response.TransferSenderId
            };
        }

        public static implicit operator BulkCreateReservations(BulkUploadAddDraftApprenticeshipRequest response)
        {
            var legalEntityId = response.LegalEntityId.HasValue ? response.LegalEntityId.Value : 0;
            return new BulkCreateReservations
            {
                CourseId = response.CourseCode,
                AccountLegalEntityId = legalEntityId,
                ProviderId = (uint?)response.ProviderId,
                RowNumber = response.RowNumber,
                Id = Guid.NewGuid(),
                StartDate = GetStartDate(response.StartDateAsString),
                TransferSenderAccountId = response.TransferSenderId,
                ULN = response.Uln
            };
        }

        public static DateTime? GetStartDate(string date, string format= "yyyy-MM-dd")
        {
            if (!string.IsNullOrWhiteSpace(date) &&
                DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outDateTime))
            {
                return new DateTime(outDateTime.Year, outDateTime.Month, 1);
            }

            return null;
        }
    }
}
