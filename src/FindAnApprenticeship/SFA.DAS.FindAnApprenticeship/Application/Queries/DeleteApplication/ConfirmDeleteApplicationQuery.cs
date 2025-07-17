using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.DeleteApplication;

public record ConfirmDeleteApplicationQuery(Guid CandidateId, Guid ApplicationId): IRequest<ConfirmDeleteApplicationQueryResult>;