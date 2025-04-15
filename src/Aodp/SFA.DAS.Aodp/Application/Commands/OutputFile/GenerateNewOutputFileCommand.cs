using MediatR;

namespace SFA.DAS.AODP.Application.Commands.OutputFile;

public class GenerateNewOutputFileCommand : IRequest<BaseMediatrResponse<EmptyResponse>> { }