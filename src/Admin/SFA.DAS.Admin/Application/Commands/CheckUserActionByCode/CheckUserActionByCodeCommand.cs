using MediatR;

namespace SFA.DAS.Admin.Application.Commands.CheckUserActionByCode
{
    public class CheckUserActionByCodeCommand : IRequest<CheckUserActionByCodeResult>
    {
        public string Code { get; set; }
        public string Username { get; set; }
    }
}
