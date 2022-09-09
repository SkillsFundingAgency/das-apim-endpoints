namespace SFA.DAS.TrackProgress.Application.DTOs;

public class ProgressDto
{
    public ProgressDetails? Progress { get; set; }

    public class ProgressDetails
    {
        public List<Ksb>? Ksbs { get; set; }
    }

    public class Ksb
    {
        /// <summary>
        /// The ID of the Knowledge, Skill or Behaviour that progress is being recorded for, as defined by IFATE within the IFATE public API (e.g. 0475bd24-fb07-4059-a37d-6463de10b45c).
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// The current progress made toward completing the KSB identified by the ID.  Value must be in the range of 0 to 100 as described below.
        ///
        /// 0 - not started
        ///
        /// 2-99 - in progress (higher number denotes more progress toward completion)
        ///
        /// 100 - completed and signed off
        /// </summary>
        public int? Value { get; set; }
    }
}