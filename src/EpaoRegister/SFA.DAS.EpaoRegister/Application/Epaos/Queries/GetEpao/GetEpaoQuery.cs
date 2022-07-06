using MediatR;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao
{
    public class GetEpaoQuery : IRequest<GetEpaoResult>
    {
        public string EpaoId { get; set; }
    }
}