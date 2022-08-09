using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails
{
    public class UpdateContactDetailsCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string ContactUsEmail { get; set; }
        public string ContactUsPhoneNumber { get; set; }
        public string ContactUsPageUrl { get; set; }
        public string StandardInfoUrl { get; set; }
    }
}
