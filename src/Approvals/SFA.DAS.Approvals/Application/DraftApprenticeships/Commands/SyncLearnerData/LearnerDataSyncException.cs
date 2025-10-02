using System;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;

public class LearnerDataSyncException(string message) : Exception(message);
