using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.StudentData
{
    public class CreateStudentDataCommand : IRequest<Unit>
    {
        public StudentDataList StudentDataList { get; set; }
    }
}