using System;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class PutCertificatePrintRequest : IPutApiRequest<PutCertificatePrintRequestData>
    {
        public PutCertificatePrintRequestData Data { get; set; }

        public string PutUrl { get; }

        public PutCertificatePrintRequest(PutCertificatePrintRequestData data, string certificateId)
        {
            Data = data;
            PutUrl = $"api/v1/certificates/{certificateId}/printrequest";
        }
    }

    public class PutCertificatePrintRequestData
    {
        public PrintAddress Address { get; set; }
        public DateTime PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }

        public static implicit operator PutCertificatePrintRequestData(CreateCertificatePrintRequestCommand command)
        {
            var addr = command?.Address == null ? null : new PrintAddress
            {
                ContactName = command.Address.ContactName,
                ContactOrganisation = command.Address.ContactOrganisation,
                ContactAddLine1 = command.Address.ContactAddLine1,
                ContactAddLine2 = command.Address.ContactAddLine2,
                ContactAddLine3 = command.Address.ContactAddLine3,
                ContactAddLine4 = command.Address.ContactAddLine4,
                ContactPostCode = command.Address.ContactPostCode
            };

            return new PutCertificatePrintRequestData
            {
                Address = addr,
                PrintRequestedAt = DateTime.UtcNow,
                PrintRequestedBy = command?.Address?.ContactName
            };
        }
    }

    public class PrintAddress
    {
        public string ContactName { get; set; }
        public string ContactOrganisation { get; set; }
        public string ContactAddLine1 { get; set; }
        public string ContactAddLine2 { get; set; }
        public string ContactAddLine3 { get; set; }
        public string ContactAddLine4 { get; set; }
        public string ContactPostCode { get; set; }
    }
}
