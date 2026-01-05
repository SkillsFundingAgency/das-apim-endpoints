using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentOnboardData
{
    public class CreateStudentOnboardDataCommand : IRequest<CreateStudentOnboardDataCommandResult>
    {
        public EmailData Emails { get; set; }
    }
}