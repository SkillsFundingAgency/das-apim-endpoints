using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CacheStandard
{
    public class CacheStandardCommand : IRequest<CacheStandardResult>
    {
        public string StandardLarsCode { get; set; }
    }
}
