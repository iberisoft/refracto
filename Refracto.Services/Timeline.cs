using System;
using System.Collections.Generic;

namespace Refracto.Services
{
    public class Timeline
    {
        public Timeline(string id, DateTime timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }

        public string Id { get; }

        public DateTime Timestamp { get; }

        public List<Readout> Data { get; } = new List<Readout>();

        public override string ToString()
        {
            return Id;
        }
    }
}
