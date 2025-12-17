using System.Collections.Generic;

namespace LiveKit
{
    /// <summary>
    /// Represents a segment of a transcription.
    /// </summary>
    public class TranscriptionSegment
    {
        /// <summary>
        /// Unique identifier for the segment.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The text of the transcription segment.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Start time of the segment in milliseconds.
        /// </summary>
        public ulong StartTime { get; }

        /// <summary>
        /// End time of the segment in milliseconds.
        /// </summary>
        public ulong EndTime { get; }

        /// <summary>
        /// The language code of the transcription.
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Whether this segment is final and will not change.
        /// </summary>
        public bool Final { get; }

        internal TranscriptionSegment(Proto.TranscriptionSegment proto)
        {
            Id = proto.Id;
            Text = proto.Text;
            StartTime = proto.StartTime;
            EndTime = proto.EndTime;
            Language = proto.Language;
            Final = proto.Final;
        }
    }

    /// <summary>
    /// Represents a transcription received from a participant.
    /// </summary>
    public class Transcription
    {
        /// <summary>
        /// The identity of the participant who produced the transcription.
        /// </summary>
        public string ParticipantIdentity { get; }

        /// <summary>
        /// The SID of the track associated with the transcription.
        /// </summary>
        public string TrackSid { get; }

        /// <summary>
        /// The segments associated with this transcription.
        /// </summary>
        public List<TranscriptionSegment> Segments { get; } = new();

        internal Transcription(Proto.TranscriptionReceived proto)
        {
            ParticipantIdentity = proto.ParticipantIdentity;
            TrackSid = proto.TrackSid;
            if (proto.Segments == null) return;
            foreach (var segment in proto.Segments)
            {
                Segments.Add(new TranscriptionSegment(segment));
            }
        }
    }
}
