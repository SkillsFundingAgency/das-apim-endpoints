using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application
{
    [ExcludeFromCodeCoverage]
    public class BulkApplicationMessageCommand : IRequest<BaseMediatrResponse<BulkApplicationMessageCommandResponse>>
    {
        public List<Guid> ApplicationIds { get; set; } = new(); 

        public bool ShareWithSkillsEngland { get; set; }
        public bool ShareWithOfqual { get; set; }
        public bool Unlock { get; set; }

        public required string UserType { get; set; }
        public required string SentByName { get; set; }
        public required string SentByEmail { get; set; }
    }
}